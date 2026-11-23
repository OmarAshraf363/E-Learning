using System.Net;
using System.Net.Http.Json;
using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.Courses;
using BFCAI.Application.Features.Instructor;

namespace BFCAI.Api.IntegrationTests;

public class InstructorTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private async Task<HttpClient> NewUserAsync(string role)
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail(role);
        await admin.PostAsJsonAsync("/api/admin/users", new { userName = email, email, role, fullName = role + " X", password = "Pass@123" });
        return await factory.AuthenticatedAsync(email, "Pass@123");
    }

    private static MultipartFormDataContent CourseForm(string name, string? instructorId = null)
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent(name), "CourseName" }, { new StringContent("desc"), "Description" },
            { new StringContent("3"), "Credits" }, { new StringContent("50"), "Price" },
            { new StringContent("https://e.com/v"), "DemoVideoUrl" }, { new StringContent("2025-01-01"), "StartDate" },
            { new StringContent("2025-02-01"), "EndDate" }, { new StringContent("1"), "DepartmentId" },
            { new ByteArrayContent([1]), "ImgCover", "c.png" }, { new StringContent("web"), "KeyWords" },
        };
        if (instructorId is not null) form.Add(new StringContent(instructorId), "InstructorId");
        return form;
    }

    private static async Task<CourseFormDto> CreateCourseAsync(HttpClient instructor, string? spoofInstructor = null)
    {
        var response = await instructor.PostAsync("/api/instructor/courses", CourseForm($"IC {Guid.NewGuid():N}", spoofInstructor));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CourseFormDto>())!;
    }

    [Fact]
    public async Task Students_cannot_use_the_instructor_api()
    {
        var student = await NewUserAsync("Student");
        Assert.Equal(HttpStatusCode.Forbidden, (await student.GetAsync("/api/instructor/dashboard")).StatusCode);
    }

    [Fact]
    public async Task Course_is_always_created_for_the_logged_in_instructor()
    {
        var instructor = await NewUserAsync("Instructor");
        var course = await CreateCourseAsync(instructor, spoofInstructor: "2"); // tries to assign it to someone else

        var mine = await instructor.GetFromJsonAsync<PagedResult<InstructorCourseRowDto>>("/api/instructor/courses?pageSize=20");
        Assert.Contains(mine!.Items, c => c.Id == course.Id);
        Assert.NotEqual("2", course.InstructorId);

        var dashboard = await instructor.GetFromJsonAsync<InstructorDashboardDto>("/api/instructor/dashboard");
        Assert.Equal(1, dashboard!.TotalCourses);
    }

    [Fact]
    public async Task Another_instructor_cannot_edit_or_read_my_course_content()
    {
        var owner = await NewUserAsync("Instructor");
        var other = await NewUserAsync("Instructor");
        var course = await CreateCourseAsync(owner);

        Assert.Equal(HttpStatusCode.Forbidden, (await other.PutAsync($"/api/instructor/courses/{course.Id}", CourseForm("hijack"))).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await other.GetAsync($"/api/instructor/courses/{course.Id}/curriculum")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await other.PostAsJsonAsync($"/api/instructor/courses/{course.Id}/sections",
            new CurriculumSectionRequest("x", "y"))).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await other.GetAsync($"/api/instructor/courses/{course.Id}/students")).StatusCode);
    }

    [Fact]
    public async Task Curriculum_sections_references_videos_and_quizzes()
    {
        var instructor = await NewUserAsync("Instructor");
        var course = await CreateCourseAsync(instructor);

        var section = await (await instructor.PostAsJsonAsync($"/api/instructor/courses/{course.Id}/sections",
            new CurriculumSectionRequest("Week 1", "Intro"))).Content.ReadFromJsonAsync<CurriculumSectionDto>();

        var reference = await (await instructor.PostAsJsonAsync($"/api/instructor/sections/{section!.Id}/references",
            new ReferenceRequest("Docs", "https://angular.dev"))).Content.ReadFromJsonAsync<CurriculumReferenceDto>();
        await instructor.PutAsJsonAsync($"/api/instructor/references/{reference!.Id}", new ReferenceRequest("Angular docs", "https://angular.dev"));

        using var video = new MultipartFormDataContent { { new StringContent("Lesson 1"), "Title" }, { new ByteArrayContent([0, 0, 1]), "Video", "lesson.mp4" } };
        var uploaded = await (await instructor.PostAsync($"/api/instructor/sections/{section.Id}/videos", video)).Content.ReadFromJsonAsync<CurriculumVideoDto>();
        Assert.StartsWith("videos/", uploaded!.Url);

        using var badVideo = new MultipartFormDataContent { { new StringContent("x"), "Title" }, { new ByteArrayContent([1]), "Video", "x.exe" } };
        Assert.Equal(HttpStatusCode.BadRequest, (await instructor.PostAsync($"/api/instructor/sections/{section.Id}/videos", badVideo)).StatusCode);

        var quiz = await (await instructor.PostAsJsonAsync($"/api/instructor/sections/{section.Id}/quizzes",
            new QuizRequest("Quiz 1", new DateTime(2025, 3, 1)))).Content.ReadFromJsonAsync<CurriculumExamDto>();

        var curriculum = await instructor.GetFromJsonAsync<CourseCurriculumDto>($"/api/instructor/courses/{course.Id}/curriculum");
        var loaded = Assert.Single(curriculum!.Sections);
        Assert.Equal("Angular docs", Assert.Single(loaded.References).Title);
        Assert.Single(loaded.Videos);
        Assert.Equal(quiz!.Id, Assert.Single(loaded.Exams).Id);

        Assert.Equal(HttpStatusCode.NoContent, (await instructor.DeleteAsync($"/api/instructor/videos/{uploaded.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await instructor.DeleteAsync($"/api/instructor/references/{reference.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await instructor.DeleteAsync($"/api/instructor/quizzes/{quiz.Id}")).StatusCode);
    }

    [Fact]
    public async Task Exams_and_question_builder()
    {
        var instructor = await NewUserAsync("Instructor");
        var course = await CreateCourseAsync(instructor);

        Assert.Equal(HttpStatusCode.Forbidden, (await instructor.PostAsJsonAsync("/api/instructor/exams",
            new ExamRequest("Not mine", DateTime.Today, 1))).StatusCode); // seed course 1 belongs to instructor1

        var exam = await (await instructor.PostAsJsonAsync("/api/instructor/exams",
            new ExamRequest("Midterm", DateTime.Today, course.Id))).Content.ReadFromJsonAsync<ExamDto>();

        var question = await (await instructor.PostAsJsonAsync($"/api/instructor/exams/{exam!.Id}/questions",
            new QuestionRequest("2 + 2 = ?", [new("4", true), new("5", false), new("", false)]))).Content.ReadFromJsonAsync<QuestionDto>();
        Assert.Equal(2, question!.Choices.Count); // empty choice ignored

        // Business rule: at least two choices and at least one correct answer (several correct answers allowed).
        Assert.Equal(HttpStatusCode.BadRequest, (await instructor.PutAsJsonAsync($"/api/instructor/questions/{question.Id}",
            new QuestionRequest("2 + 3 = ?", [new("5", true), new("", false)]))).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await instructor.PutAsJsonAsync($"/api/instructor/questions/{question.Id}",
            new QuestionRequest("2 + 3 = ?", [new("5", false), new("6", false)]))).StatusCode);

        var updated = await (await instructor.PutAsJsonAsync($"/api/instructor/questions/{question.Id}",
            new QuestionRequest("2 + 3 = ?", [new("5", true), new("five", true), new("6", false)]))).Content.ReadFromJsonAsync<QuestionDto>();
        Assert.Equal("2 + 3 = ?", updated!.QuestionText);
        Assert.Equal(2, updated.Choices.Count(c => c.IsCorrect));

        var exams = await instructor.GetFromJsonAsync<List<ExamRowDto>>("/api/instructor/exams");
        Assert.Equal(1, Assert.Single(exams!).TotalQuestions);
        Assert.Equal(HttpStatusCode.NoContent, (await instructor.DeleteAsync($"/api/instructor/exams/{exam.Id}")).StatusCode);
    }

    [Fact]
    public async Task Assignments_file_required_on_create_optional_on_edit()
    {
        var instructor = await NewUserAsync("Instructor");
        var course = await CreateCourseAsync(instructor);
        var section = await (await instructor.PostAsJsonAsync($"/api/instructor/courses/{course.Id}/sections",
            new CurriculumSectionRequest("S", "C"))).Content.ReadFromJsonAsync<CurriculumSectionDto>();

        using var noFile = new MultipartFormDataContent { { new StringContent("HW1"), "Title" }, { new StringContent("2025-05-01"), "DeadLine" } };
        Assert.Equal(HttpStatusCode.BadRequest, (await instructor.PostAsync($"/api/instructor/sections/{section!.Id}/assignments", noFile)).StatusCode);

        using var create = new MultipartFormDataContent
        {
            { new StringContent("HW1"), "Title" }, { new StringContent("2025-05-01"), "DeadLine" },
            { new ByteArrayContent([1, 2]), "Content", "hw.pdf" },
        };
        var assignment = await (await instructor.PostAsync($"/api/instructor/sections/{section.Id}/assignments", create))
            .Content.ReadFromJsonAsync<AssignmentDto>();
        Assert.StartsWith("/assignments/", assignment!.Content);

        using var edit = new MultipartFormDataContent { { new StringContent("HW1 v2"), "Title" }, { new StringContent("2025-05-02"), "DeadLine" } };
        var edited = await (await instructor.PutAsync($"/api/instructor/assignments/{assignment.Id}", edit)).Content.ReadFromJsonAsync<AssignmentDto>();
        Assert.Equal("HW1 v2", edited!.Title);
        Assert.Equal(assignment.Content, edited.Content);

        Assert.Empty((await instructor.GetFromJsonAsync<List<SubmissionDto>>($"/api/instructor/assignments/{assignment.Id}/submissions"))!);
        var byCourse = await instructor.GetFromJsonAsync<List<SectionAssignmentsDto>>($"/api/instructor/courses/{course.Id}/assignments");
        Assert.Single(Assert.Single(byCourse!).Assignments);
    }

    [Fact]
    public async Task Students_of_my_course_and_feedback()
    {
        var instructor = await NewUserAsync("Instructor");
        var course = await CreateCourseAsync(instructor);
        var admin = await factory.AsAdminAsync();
        await admin.PostAsJsonAsync("/api/admin/enrollments/by-email", new { email = "student1@example.com", courseId = course.Id });

        var students = await instructor.GetFromJsonAsync<List<CourseStudentDto>>($"/api/instructor/courses/{course.Id}/students");
        var student = Assert.Single(students!);

        var response = await instructor.PostAsJsonAsync("/api/instructor/feedback", new StudentFeedbackRequest(student.Id, "Great work"));
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Instructor_linked_to_a_department_creates_courses_only_in_it()
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail("Instructor");
        await admin.PostAsJsonAsync("/api/admin/users",
            new { userName = email, email, role = "Instructor", password = "Pass@123", departmentId = 2 });
        var instructor = await factory.AuthenticatedAsync(email, "Pass@123");

        var lookups = await instructor.GetFromJsonAsync<InstructorCourseLookupsDto>("/api/instructor/courses/form-lookups");
        Assert.Equal(2, Assert.Single(lookups!.Departments).Id);

        var response = await instructor.PostAsync("/api/instructor/courses", CourseForm($"IC {Guid.NewGuid():N}")); // department 1
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Video_title_and_file_can_be_edited()
    {
        var instructor = await NewUserAsync("Instructor");
        var course = await CreateCourseAsync(instructor);
        var section = await (await instructor.PostAsJsonAsync($"/api/instructor/courses/{course.Id}/sections",
            new CurriculumSectionRequest("S", "C"))).Content.ReadFromJsonAsync<CurriculumSectionDto>();
        using var video = new MultipartFormDataContent { { new StringContent("Lesson 1"), "Title" }, { new ByteArrayContent([0, 1]), "Video", "a.mp4" } };
        var uploaded = await (await instructor.PostAsync($"/api/instructor/sections/{section!.Id}/videos", video)).Content.ReadFromJsonAsync<CurriculumVideoDto>();

        using var titleOnly = new MultipartFormDataContent { { new StringContent("Lesson 1 (new)"), "Title" } };
        var renamed = await (await instructor.PutAsync($"/api/instructor/videos/{uploaded!.Id}", titleOnly)).Content.ReadFromJsonAsync<CurriculumVideoDto>();
        Assert.Equal("Lesson 1 (new)", renamed!.Title);
        Assert.Equal(uploaded.Url, renamed.Url);

        using var newFile = new MultipartFormDataContent { { new StringContent("Lesson 1"), "Title" }, { new ByteArrayContent([0, 2]), "Video", "b.mp4" } };
        var replaced = await (await instructor.PutAsync($"/api/instructor/videos/{uploaded.Id}", newFile)).Content.ReadFromJsonAsync<CurriculumVideoDto>();
        Assert.NotEqual(uploaded.Url, replaced!.Url);

        var other = await NewUserAsync("Instructor");
        using var steal = new MultipartFormDataContent { { new StringContent("x"), "Title" } };
        Assert.Equal(HttpStatusCode.Forbidden, (await other.PutAsync($"/api/instructor/videos/{uploaded.Id}", steal)).StatusCode);
    }

    [Fact]
    public async Task Sections_can_be_reordered_and_deleted_with_their_content()
    {
        var instructor = await NewUserAsync("Instructor");
        var course = await CreateCourseAsync(instructor);
        var ids = new List<int>();
        foreach (var title in new[] { "A", "B", "C" })
        {
            var created = await (await instructor.PostAsJsonAsync($"/api/instructor/courses/{course.Id}/sections",
                new CurriculumSectionRequest(title, "c"))).Content.ReadFromJsonAsync<CurriculumSectionDto>();
            ids.Add(created!.Id);
        }

        var moved = await (await instructor.PutAsJsonAsync($"/api/instructor/sections/{ids[2]}/move", new MoveSectionRequest("up")))
            .Content.ReadFromJsonAsync<CourseCurriculumDto>();
        Assert.Equal(["A", "C", "B"], moved!.Sections.Select(x => x.Title));
        Assert.Equal(HttpStatusCode.BadRequest, (await instructor.PutAsJsonAsync($"/api/instructor/sections/{ids[0]}/move",
            new MoveSectionRequest("left"))).StatusCode);

        // Section "C" with a video, a reference, an assignment and a quiz
        using var video = new MultipartFormDataContent { { new StringContent("V"), "Title" }, { new ByteArrayContent([0, 1]), "Video", "v.mp4" } };
        await instructor.PostAsync($"/api/instructor/sections/{ids[2]}/videos", video);
        await instructor.PostAsJsonAsync($"/api/instructor/sections/{ids[2]}/references", new ReferenceRequest("R", "https://e.com"));
        using var hw = new MultipartFormDataContent { { new StringContent("HW"), "Title" }, { new StringContent("2025-05-01"), "DeadLine" }, { new ByteArrayContent([1]), "Content", "hw.pdf" } };
        await instructor.PostAsync($"/api/instructor/sections/{ids[2]}/assignments", hw);
        var quiz = await (await instructor.PostAsJsonAsync($"/api/instructor/sections/{ids[2]}/quizzes",
            new QuizRequest("Q", DateTime.Today))).Content.ReadFromJsonAsync<CurriculumExamDto>();
        await instructor.PostAsJsonAsync($"/api/instructor/exams/{quiz!.Id}/questions", new QuestionRequest("?", [new("a", true), new("b", false)]));

        var summary = await instructor.GetFromJsonAsync<SectionDeleteSummaryDto>($"/api/instructor/sections/{ids[2]}/delete-summary");
        Assert.Equal(new SectionDeleteSummaryDto(1, 1, 1, 1, 0), summary);

        var other = await NewUserAsync("Instructor");
        Assert.Equal(HttpStatusCode.Forbidden, (await other.DeleteAsync($"/api/instructor/sections/{ids[2]}")).StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await instructor.DeleteAsync($"/api/instructor/sections/{ids[2]}")).StatusCode);
        var curriculum = await instructor.GetFromJsonAsync<CourseCurriculumDto>($"/api/instructor/courses/{course.Id}/curriculum");
        Assert.Equal(["A", "B"], curriculum!.Sections.Select(x => x.Title));
    }
}

using System.Net;
using System.Net.Http.Json;
using BFCAI.Application.Features.Catalog;
using BFCAI.Application.Features.Courses;
using BFCAI.Application.Features.Instructor;
using BFCAI.Application.Features.Student;

namespace BFCAI.Api.IntegrationTests;

public class StudentTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private record Setup(HttpClient Student, string StudentEmail, int CourseId, int SectionId, int AssignmentId);

    private async Task<(HttpClient Client, string Email)> NewUserAsync(string role)
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail(role);
        await admin.PostAsJsonAsync("/api/admin/users", new { userName = email, email, role, fullName = role + " Y", password = "Pass@123" });
        return (await factory.AuthenticatedAsync(email, "Pass@123"), email);
    }

    /// <summary>Instructor builds a course (section + assignment + quiz with a question); a student is enrolled.</summary>
    private async Task<Setup> SetupAsync()
    {
        var (instructor, _) = await NewUserAsync("Instructor");
        using var form = new MultipartFormDataContent
        {
            { new StringContent($"SC {Guid.NewGuid():N}"), "CourseName" }, { new StringContent("d"), "Description" },
            { new StringContent("3"), "Credits" }, { new StringContent("https://e.com/v"), "DemoVideoUrl" },
            { new StringContent("2025-01-01"), "StartDate" }, { new StringContent("2025-02-01"), "EndDate" },
            { new StringContent("1"), "DepartmentId" }, { new ByteArrayContent([1]), "ImgCover", "c.png" },
        };
        var course = await (await instructor.PostAsync("/api/instructor/courses", form)).Content.ReadFromJsonAsync<CourseFormDto>();
        var section = await (await instructor.PostAsJsonAsync($"/api/instructor/courses/{course!.Id}/sections",
            new CurriculumSectionRequest("Week 1", "Intro"))).Content.ReadFromJsonAsync<CurriculumSectionDto>();

        using var hw = new MultipartFormDataContent
        {
            { new StringContent("HW"), "Title" }, { new StringContent("2025-05-01"), "DeadLine" }, { new ByteArrayContent([1]), "Content", "hw.pdf" },
        };
        var assignment = await (await instructor.PostAsync($"/api/instructor/sections/{section!.Id}/assignments", hw)).Content.ReadFromJsonAsync<AssignmentDto>();

        var quiz = await (await instructor.PostAsJsonAsync($"/api/instructor/sections/{section.Id}/quizzes",
            new QuizRequest("Q", DateTime.Today))).Content.ReadFromJsonAsync<CurriculumExamDto>();
        await instructor.PostAsJsonAsync($"/api/instructor/exams/{quiz!.Id}/questions",
            new QuestionRequest("1+1?", [new("2", true), new("3", false)]));

        var (student, email) = await NewUserAsync("Student");
        var admin = await factory.AsAdminAsync();
        await admin.PostAsJsonAsync("/api/admin/enrollments/by-email", new { email, courseId = course.Id });

        return new Setup(student, email, course.Id, section.Id, assignment!.Id);
    }

    [Fact]
    public async Task Dashboard_and_my_courses_list_the_enrolled_course()
    {
        var s = await SetupAsync();
        var dashboard = await s.Student.GetFromJsonAsync<StudentDashboardDto>("/api/student/dashboard");
        Assert.Contains(dashboard!.Courses, c => c.Id == s.CourseId);

        var mine = await s.Student.GetFromJsonAsync<List<MyCourseDto>>("/api/student/courses");
        Assert.Contains(mine!, c => c.Id == s.CourseId);
    }

    [Fact]
    public async Task Course_content_requires_enrollment()
    {
        var s = await SetupAsync();
        var (outsider, _) = await NewUserAsync("Student");

        Assert.Equal(HttpStatusCode.Forbidden, (await outsider.GetAsync($"/api/student/courses/{s.CourseId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await outsider.GetAsync($"/api/student/sections/{s.SectionId}")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await s.Student.GetAsync($"/api/student/courses/{s.CourseId}")).StatusCode);
    }

    [Fact]
    public async Task Section_content_quiz_and_quick_access()
    {
        var s = await SetupAsync();
        var content = await s.Student.GetFromJsonAsync<SectionContentDto>($"/api/student/sections/{s.SectionId}");
        Assert.True(content!.HasQuiz);
        Assert.False(Assert.Single(content.Assignments).Submitted);

        var response = await s.Student.GetAsync($"/api/student/sections/{s.SectionId}/quiz");
        var raw = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("isCorrect", raw, StringComparison.OrdinalIgnoreCase); // answers are never sent
        var quiz = System.Text.Json.JsonSerializer.Deserialize<List<QuizQuestionDto>>(raw, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));
        Assert.Equal(["2", "3"], Assert.Single(quiz!).Choices);

        var quick = await s.Student.GetFromJsonAsync<QuickAccessDto>($"/api/student/courses/{s.CourseId}/quick-access");
        Assert.Single(quick!.Sections);
    }

    [Fact]
    public async Task Assignment_can_be_submitted_once()
    {
        var s = await SetupAsync();
        var first = await s.Student.PostAsJsonAsync($"/api/student/assignments/{s.AssignmentId}/submissions", new SubmitAssignmentRequest("https://github.com/me/hw"));
        Assert.Equal(HttpStatusCode.NoContent, first.StatusCode);

        var second = await s.Student.PostAsJsonAsync($"/api/student/assignments/{s.AssignmentId}/submissions", new SubmitAssignmentRequest("again"));
        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);

        var content = await s.Student.GetFromJsonAsync<SectionContentDto>($"/api/student/sections/{s.SectionId}");
        Assert.True(Assert.Single(content!.Assignments).Submitted);
    }

    [Fact]
    public async Task Feedback_updates_course_rate_and_only_the_owner_can_change_it()
    {
        var s = await SetupAsync();
        var created = await (await s.Student.PostAsJsonAsync("/api/student/feedback",
            new FeedbackRequest(s.CourseId, null, 4, "Nice"))).Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        var feedbackId = created.GetProperty("id").GetInt32();

        var details = await factory.CreateClient().GetFromJsonAsync<CourseDetailsDto>($"/api/catalog/courses/{s.CourseId}");
        Assert.Equal(4, details!.Rate);

        await s.Student.PutAsJsonAsync($"/api/student/feedback/{feedbackId}", new FeedbackRequest(s.CourseId, null, 2, "Changed"));
        details = await factory.CreateClient().GetFromJsonAsync<CourseDetailsDto>($"/api/catalog/courses/{s.CourseId}");
        Assert.Equal(2, details!.Rate);

        var (other, otherEmail) = await NewUserAsync("Student");
        Assert.Equal(HttpStatusCode.Forbidden, (await other.DeleteAsync($"/api/student/feedback/{feedbackId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await s.Student.DeleteAsync($"/api/student/feedback/{feedbackId}")).StatusCode);
    }

    [Fact]
    public async Task Instructors_cannot_use_the_student_api()
    {
        var (instructor, _) = await NewUserAsync("Instructor");
        Assert.Equal(HttpStatusCode.Forbidden, (await instructor.GetAsync("/api/student/dashboard")).StatusCode);
    }
}

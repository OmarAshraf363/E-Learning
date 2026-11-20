using System.Net;
using System.Net.Http.Json;
using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.AcademicYears;
using BFCAI.Application.Features.Courses;
using BFCAI.Application.Features.Dashboard;
using BFCAI.Application.Features.Departments;
using BFCAI.Application.Features.Enrollments;
using BFCAI.Application.Features.Schedules;
using BFCAI.Application.Features.Tracks;

namespace BFCAI.Api.IntegrationTests;

public class AdminCatalogTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private static ByteArrayContent Png() => new([137, 80, 78, 71]);

    private static MultipartFormDataContent CourseForm(string name, bool withCover, params (string Key, string Value)[] extra)
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent(name), "CourseName" },
            { new StringContent("A course"), "Description" },
            { new StringContent("3"), "Credits" },
            { new StringContent("100"), "Price" },
            { new StringContent("https://example.com/demo.mp4"), "DemoVideoUrl" },
            { new StringContent("2025-01-01"), "StartDate" },
            { new StringContent("2025-06-01"), "EndDate" },
            { new StringContent("2"), "InstructorId" },
            { new StringContent("1"), "DepartmentId" },
        };
        if (withCover) form.Add(Png(), "ImgCover", "cover.png");
        foreach (var (key, value) in extra) form.Add(new StringContent(value), key);
        return form;
    }

    [Fact]
    public async Task Department_cover_is_required_on_create_and_kept_on_edit()
    {
        var admin = await factory.AsAdminAsync();

        var noCover = await admin.PostAsync("/api/admin/departments", new MultipartFormDataContent
        {
            { new StringContent("AI"), "DepartmentName" }, { new StringContent("Artificial intelligence"), "DepartmentDescription" },
        });
        Assert.Equal(HttpStatusCode.BadRequest, noCover.StatusCode);

        var created = await (await admin.PostAsync("/api/admin/departments", new MultipartFormDataContent
        {
            { new StringContent("AI"), "DepartmentName" }, { new StringContent("Artificial intelligence"), "DepartmentDescription" },
            { Png(), "Cover", "ai.png" },
        })).Content.ReadFromJsonAsync<DepartmentDto>();
        Assert.StartsWith("/Covers/", created!.Cover);

        var edited = await (await admin.PutAsync($"/api/admin/departments/{created.Id}", new MultipartFormDataContent
        {
            { new StringContent("AI & ML"), "DepartmentName" }, { new StringContent("desc"), "DepartmentDescription" },
        })).Content.ReadFromJsonAsync<DepartmentDto>();
        Assert.Equal("AI & ML", edited!.Name);
        Assert.Equal(created.Cover, edited.Cover);
    }

    [Fact]
    public async Task Department_with_users_cannot_be_deleted_returns_409()
    {
        var admin = await factory.AsAdminAsync();
        // Seed: department 1 has courses -> FK conflict
        var response = await admin.DeleteAsync("/api/admin/departments/1");
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Department_details_include_courses_and_schedules()
    {
        var admin = await factory.AsAdminAsync();
        var details = await admin.GetFromJsonAsync<DepartmentDetailsDto>("/api/admin/departments/2/details");
        Assert.Contains(details!.Courses, c => c.Name == "Digital Circuits");
    }

    [Fact]
    public async Task Course_create_and_update_replace_keywords_objectives_topics_and_keep_cover()
    {
        var admin = await factory.AsAdminAsync();

        var created = await (await admin.PostAsync("/api/admin/courses", CourseForm("Angular", true,
            ("KeyWords", "web"), ("KeyWords", "spa"), ("LearningObjectives", "Build apps"), ("TopicsCovered", "Signals"), ("KeyWords", " "))))
            .Content.ReadFromJsonAsync<CourseFormDto>();
        Assert.Equal(["web", "spa"], created!.KeyWords);
        Assert.NotNull(created.ImgCover);

        var updated = await (await admin.PutAsync($"/api/admin/courses/{created.Id}", CourseForm("Angular 22", false,
            ("KeyWords", "frontend"), ("TopicsCovered", "Routing"), ("TopicsCovered", "Forms"))))
            .Content.ReadFromJsonAsync<CourseFormDto>();
        Assert.Equal("Angular 22", updated!.CourseName);
        Assert.Equal(["frontend"], updated.KeyWords);
        Assert.Empty(updated.LearningObjectives);
        Assert.Equal(["Routing", "Forms"], updated.TopicsCovered);
        Assert.Equal(created.ImgCover, updated.ImgCover);
    }

    [Fact]
    public async Task Course_validation_matches_legacy_rules()
    {
        var admin = await factory.AsAdminAsync();
        var form = new MultipartFormDataContent { { new StringContent("0"), "Credits" } };
        var response = await admin.PostAsync("/api/admin/courses", form);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Credits must be between 1 and 50.", body);
    }

    [Fact]
    public async Task Track_crud()
    {
        var admin = await factory.AsAdminAsync();
        var created = await (await admin.PostAsync("/api/admin/tracks", new MultipartFormDataContent
        {
            { new StringContent("Backend"), "Name" }, { new StringContent(".NET track"), "Description" }, { Png(), "Cover", "b.png" },
        })).Content.ReadFromJsonAsync<TrackDto>();

        var list = await admin.GetFromJsonAsync<List<TrackListItemDto>>("/api/admin/tracks");
        Assert.Contains(list!, t => t.Id == created!.Id);

        Assert.Equal(HttpStatusCode.NoContent, (await admin.DeleteAsync($"/api/admin/tracks/{created!.Id}")).StatusCode);
    }

    [Fact]
    public async Task Academic_year_is_saved()
    {
        var admin = await factory.AsAdminAsync();
        await admin.PostAsJsonAsync("/api/admin/academic-years", new AcademicYearRequest("2024-2025"));
        var years = await admin.GetFromJsonAsync<List<AcademicYearDto>>("/api/admin/academic-years");
        Assert.Contains(years!, y => y.YearName == "2024-2025");
    }

    [Fact]
    public async Task Schedule_with_guid_instructor_can_be_created_and_edited()
    {
        var admin = await factory.AsAdminAsync();
        var created = await (await admin.PostAsJsonAsync("/api/admin/schedules",
            new ScheduleRequest("09:00", "11:30", "Sunday", 1, "123548458", 1, 1, 1))).Content.ReadFromJsonAsync<ScheduleFormDto>();
        Assert.Equal("09:00", created!.StartTime);
        Assert.Equal("123548458", created.InstructorId);

        var updated = await (await admin.PutAsJsonAsync($"/api/admin/schedules/{created.Id}",
            new ScheduleRequest("10:00", "12:00", "Monday", 2, "2", null, null, null))).Content.ReadFromJsonAsync<ScheduleFormDto>();
        Assert.Equal("Monday", updated!.DayOfWeek);
    }

    [Fact]
    public async Task Enroll_by_email_and_csv()
    {
        var admin = await factory.AsAdminAsync();
        var unknown = await admin.PostAsJsonAsync("/api/admin/enrollments/by-email", new EnrollByEmailRequest("nobody@x.com", 1));
        Assert.Equal(HttpStatusCode.BadRequest, unknown.StatusCode);

        var ok = await admin.PostAsJsonAsync("/api/admin/enrollments/by-email", new EnrollByEmailRequest("student1@example.com", 2));
        Assert.Equal(HttpStatusCode.NoContent, ok.StatusCode);

        var csv = new MultipartFormDataContent
        {
            { new ByteArrayContent("1,1\nstudent1@example.com,2\nbad line\n1,999\n"u8.ToArray()), "csvFile", "students.csv" },
        };
        var result = await (await admin.PostAsync("/api/admin/enrollments/csv", csv)).Content.ReadFromJsonAsync<CsvImportResultDto>();
        Assert.Equal(2, result!.Added);
        Assert.Equal(2, result.Skipped);

        var overview = await admin.GetFromJsonAsync<EnrollmentOverviewDto>("/api/admin/enrollments");
        Assert.True(overview!.TotalEnrollments >= 5);
    }

    [Fact]
    public async Task Dashboard_returns_real_totals()
    {
        var admin = await factory.AsAdminAsync();
        var dashboard = await admin.GetFromJsonAsync<AdminDashboardDto>("/api/admin/dashboard?page=1&pageSize=3");
        Assert.True(dashboard!.TotalCourses >= 2);
        Assert.Equal(dashboard.TotalCourses, dashboard.Courses.TotalCount);
        Assert.NotNull(dashboard.MostPopularCourse);
    }

    [Fact]
    public async Task Lookups_return_instructors_by_user_type()
    {
        var admin = await factory.AsAdminAsync();
        var instructors = await admin.GetFromJsonAsync<List<LookupItem<string>>>("/api/admin/lookups/instructors");
        Assert.Contains(instructors!, i => i.Id == "2");
    }

    [Theory]
    [InlineData("/api/admin/dashboard")]
    [InlineData("/api/admin/courses")]
    [InlineData("/api/admin/departments")]
    [InlineData("/api/admin/enrollments")]
    public async Task Admin_endpoints_require_login(string url)
    {
        var response = await factory.CreateClient().GetAsync(url);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BFCAI.Application.Common;
using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.Catalog;

namespace BFCAI.Api.IntegrationTests;

public class CatalogTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient _anonymous = factory.CreateClient();

    [Fact]
    public async Task Home_works_anonymously_and_fills_the_cache()
    {
        var home = await _anonymous.GetFromJsonAsync<HomeDto>("/api/catalog/home");

        Assert.NotEmpty(home!.TopCourses);              // seed: courses 1 and 2 have enrollments
        Assert.Empty(home.Recommendations);              // only for students
        Assert.Empty(home.EnrolledCourseIds);
        Assert.True(factory.Cache.Items.ContainsKey(CacheKeys.TopCourses));
        Assert.True(factory.Cache.Items.ContainsKey(CacheKeys.TopTracks)); // legacy wrote / read different keys
    }

    [Fact]
    public async Task Home_for_a_student_returns_enrolled_course_ids()
    {
        // student1 (seed) has no password: enroll a new student through the admin API instead
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail();
        await admin.PostAsJsonAsync("/api/admin/users", new { userName = email, email, role = "Student", password = "Pass@123" });
        await admin.PostAsJsonAsync("/api/admin/enrollments/by-email", new { email, courseId = 1 });

        var student = await factory.AuthenticatedAsync(email, "Pass@123");
        var home = await student.GetFromJsonAsync<HomeDto>("/api/catalog/home");
        Assert.Contains(1, home!.EnrolledCourseIds);
    }

    [Fact]
    public async Task Courses_search_filters_are_applied()
    {
        var all = await _anonymous.GetFromJsonAsync<PagedResult<CourseCardDto>>("/api/catalog/courses?pageSize=20");
        var search = await _anonymous.GetFromJsonAsync<PagedResult<CourseCardDto>>("/api/catalog/courses?search=digital&pageSize=20");
        var category = await _anonymous.GetFromJsonAsync<PagedResult<CourseCardDto>>("/api/catalog/courses?categoryId=1&pageSize=20");

        Assert.True(all!.TotalCount >= 2);
        Assert.All(search!.Items, c => Assert.Contains("digital", c.Title, StringComparison.OrdinalIgnoreCase));
        Assert.Single(search.Items);
        Assert.DoesNotContain(category!.Items, c => c.Title == "Digital Circuits");
    }

    [Fact]
    public async Task Course_details_include_reviews_related_and_instructor_courses()
    {
        var details = await _anonymous.GetFromJsonAsync<CourseDetailsDto>("/api/catalog/courses/1");

        Assert.Equal("Introduction to Programming", details!.CourseName);
        Assert.Equal(1, details.ReviewSummary.TotalReviews);
        Assert.Null(details.DemoTitle); // seed videos are not linked to a curriculum section
        Assert.Contains(details.InstructorCourses, c => c.Id == 2);
        Assert.Equal(HttpStatusCode.NotFound, (await _anonymous.GetAsync("/api/catalog/courses/9999")).StatusCode);
    }

    [Fact]
    public async Task Topic_category_track_and_nav()
    {
        var topic = await _anonymous.GetFromJsonAsync<List<CourseCardDto>>("/api/catalog/topics/program");
        Assert.Contains(topic!, c => c.Id == 1);

        var category = await _anonymous.GetFromJsonAsync<CategoryDto>("/api/catalog/categories/2");
        Assert.Contains(category!.Courses, c => c.Title == "Digital Circuits");

        var nav = await _anonymous.GetFromJsonAsync<List<NavCategoryDto>>("/api/catalog/nav");
        Assert.Contains(nav!, d => d.Courses.Any(c => c.Id == 1));

        Assert.Equal(HttpStatusCode.NotFound, (await _anonymous.GetAsync("/api/catalog/tracks/9999")).StatusCode);
    }

    [Fact]
    public async Task Changing_a_course_clears_the_home_cache()
    {
        await _anonymous.GetAsync("/api/catalog/home");
        Assert.True(factory.Cache.Items.ContainsKey(CacheKeys.TopCourses));

        var admin = await factory.AsAdminAsync();
        await admin.DeleteAsync("/api/admin/tracks/99999"); // 404, nothing changes
        Assert.True(factory.Cache.Items.ContainsKey(CacheKeys.TopCourses));

        await admin.PostAsJsonAsync("/api/admin/academic-years", new { yearName = "x" }); // unrelated
        using var form = new MultipartFormDataContent
        {
            { new StringContent("Cache Track"), "Name" }, { new StringContent("d"), "Description" },
            { new ByteArrayContent([1]), "Cover", "c.png" },
        };
        await admin.PostAsync("/api/admin/tracks", form);
        Assert.False(factory.Cache.Items.ContainsKey(CacheKeys.TopCourses));
    }
}

using System.Net;
using System.Net.Http.Json;
using BFCAI.Application.Features.Cart;
using BFCAI.Application.Features.Catalog;

namespace BFCAI.Api.IntegrationTests;

public class CartTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private async Task<HttpClient> NewStudentAsync()
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail();
        await admin.PostAsJsonAsync("/api/admin/users", new { userName = email, email, role = "Student", password = "Pass@123" });
        return await factory.AuthenticatedAsync(email, "Pass@123");
    }

    private async Task<int> CreatePricedCourseAsync(decimal price)
    {
        var admin = await factory.AsAdminAsync();
        using var form = new MultipartFormDataContent
        {
            { new StringContent($"Paid {Guid.NewGuid():N}"), "CourseName" }, { new StringContent("d"), "Description" },
            { new StringContent("3"), "Credits" }, { new StringContent(price.ToString()), "Price" },
            { new StringContent("https://e.com/v"), "DemoVideoUrl" }, { new StringContent("2025-01-01"), "StartDate" },
            { new StringContent("2025-02-01"), "EndDate" }, { new StringContent("2"), "InstructorId" },
            { new StringContent("1"), "DepartmentId" }, { new ByteArrayContent([1]), "ImgCover", "c.png" },
        };
        var course = await (await admin.PostAsync("/api/admin/courses", form)).Content.ReadFromJsonAsync<BFCAI.Application.Features.Courses.CourseFormDto>();
        return course!.Id;
    }

    [Fact]
    public async Task Cart_requires_login()
        => Assert.Equal(HttpStatusCode.Unauthorized, (await factory.CreateClient().GetAsync("/api/cart")).StatusCode);

    [Fact]
    public async Task The_same_course_can_be_added_only_once()
    {
        var student = await NewStudentAsync();
        var courseId = await CreatePricedCourseAsync(100);

        var cart = await (await student.PostAsJsonAsync("/api/cart/items", new AddToCartRequest(courseId))).Content.ReadFromJsonAsync<CartDto>();
        var again = await student.PostAsJsonAsync("/api/cart/items", new AddToCartRequest(courseId));
        Assert.Equal(HttpStatusCode.BadRequest, again.StatusCode);

        var item = Assert.Single(cart!.Items);
        Assert.Equal(1, item.Quantity);
        Assert.Equal(100, cart.Subtotal);

        var summary = await student.GetFromJsonAsync<CartSummaryDto>("/api/cart/summary");
        Assert.Equal(1, summary!.Count);
        Assert.Equal([courseId], summary.CartCourseIds);
    }

    [Fact]
    public async Task An_enrolled_course_cannot_be_added_to_the_cart()
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail();
        await admin.PostAsJsonAsync("/api/admin/users", new { userName = email, email, role = "Student", password = "Pass@123" });
        var student = await factory.AuthenticatedAsync(email, "Pass@123");
        var courseId = await CreatePricedCourseAsync(80);

        // Added to the cart first, then enrolled by the admin: checkout removes it instead of charging again.
        await student.PostAsJsonAsync("/api/cart/items", new AddToCartRequest(courseId));
        await admin.PostAsJsonAsync("/api/admin/enrollments/by-email", new BFCAI.Application.Features.Enrollments.EnrollByEmailRequest(email, courseId));

        var checkout = await student.PostAsync("/api/cart/checkout", null);
        Assert.Equal(HttpStatusCode.BadRequest, checkout.StatusCode);
        var summary = await student.GetFromJsonAsync<CartSummaryDto>("/api/cart/summary");
        Assert.Equal(0, summary!.Count);
        Assert.Contains(courseId, summary.EnrolledCourseIds);

        var add = await student.PostAsJsonAsync("/api/cart/items", new AddToCartRequest(courseId));
        Assert.Equal(HttpStatusCode.BadRequest, add.StatusCode);
    }

    [Fact]
    public async Task A_user_cannot_remove_another_users_cart_item()
    {
        var owner = await NewStudentAsync();
        var other = await NewStudentAsync();
        var cart = await (await owner.PostAsJsonAsync("/api/cart/items", new AddToCartRequest(1))).Content.ReadFromJsonAsync<CartDto>();

        var response = await other.DeleteAsync($"/api/cart/items/{cart!.Items[0].Id}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var removed = await (await owner.DeleteAsync($"/api/cart/items/{cart.Items[0].Id}")).Content.ReadFromJsonAsync<CartDto>();
        Assert.Empty(removed!.Items);
    }

    [Fact]
    public async Task Checkout_with_empty_cart_is_rejected()
    {
        var student = await NewStudentAsync();
        var response = await student.PostAsync("/api/cart/checkout", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Unpaid_session_does_not_enroll_paid_session_enrolls_once_and_clears_cart()
    {
        var student = await NewStudentAsync();
        var courseId = await CreatePricedCourseAsync(250);
        await student.PostAsJsonAsync("/api/cart/items", new AddToCartRequest(courseId));

        var start = await (await student.PostAsync("/api/cart/checkout", null)).Content.ReadFromJsonAsync<CheckoutStartDto>();
        var sessionId = start!.Url.Split('/').Last();
        Assert.Equal(250, factory.Payments.Sessions[sessionId].Lines.Single().UnitPrice);

        // Not paid yet -> legacy would have enrolled here
        var unpaid = await student.PostAsync($"/api/cart/checkout/confirm?sessionId={sessionId}", null);
        Assert.Equal(HttpStatusCode.BadRequest, unpaid.StatusCode);

        factory.Payments.MarkPaid(sessionId);
        var result = await (await student.PostAsync($"/api/cart/checkout/confirm?sessionId={sessionId}", null))
            .Content.ReadFromJsonAsync<CheckoutResultDto>();
        Assert.Equal(250, result!.Total);

        // Refresh of the success page: same result, no second enrollment
        var again = await student.PostAsync($"/api/cart/checkout/confirm?sessionId={sessionId}", null);
        Assert.Equal(HttpStatusCode.OK, again.StatusCode);

        var home = await student.GetFromJsonAsync<HomeDto>("/api/catalog/home");
        Assert.Single(home!.EnrolledCourseIds, id => id == courseId);
        Assert.Equal(0, (await student.GetFromJsonAsync<CartSummaryDto>("/api/cart/summary"))!.Count);
    }

    [Fact]
    public async Task Another_users_session_cannot_be_confirmed()
    {
        var owner = await NewStudentAsync();
        await owner.PostAsJsonAsync("/api/cart/items", new AddToCartRequest(1));
        var start = await (await owner.PostAsync("/api/cart/checkout", null)).Content.ReadFromJsonAsync<CheckoutStartDto>();
        var sessionId = start!.Url.Split('/').Last();
        factory.Payments.MarkPaid(sessionId);

        var thief = await NewStudentAsync();
        var response = await thief.PostAsync($"/api/cart/checkout/confirm?sessionId={sessionId}", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

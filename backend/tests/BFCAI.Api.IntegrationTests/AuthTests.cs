using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BFCAI.Application.Features.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.IntegrationTests;

public class AuthTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Seeded_admin_can_login_and_read_me()
    {
        var auth = await _client.LoginAsync(TestHelpers.AdminEmail, TestHelpers.AdminPassword);
        Assert.Contains("Admin", auth.User.Roles);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
        var me = await _client.GetFromJsonAsync<CurrentUserDto>("/api/auth/me");
        Assert.Equal("Admin@gmail.com", me!.Email);
    }

    [Fact]
    public async Task Wrong_password_returns_401_problem_details()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest(TestHelpers.AdminEmail, "wrong"));
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal("Invalid login attempt.", problem!.Detail);
    }

    [Fact]
    public async Task Register_creates_student_only_and_signs_in()
    {
        var email = TestHelpers.UniqueEmail();
        var response = await _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("New Student", email, "Pass@123", "Pass@123"));

        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.Equal(["Student"], auth!.User.Roles);
        Assert.Equal("New Student", auth.User.FullName);
    }

    [Fact]
    public async Task Register_validation_errors_are_returned_per_field()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("x", "not-an-email", "Pass@123", "different"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.Contains("Email", problem!.Errors.Keys);
        Assert.Contains("ConfirmPassword", problem.Errors.Keys);
    }

    [Fact]
    public async Task Weak_password_returns_identity_errors()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("x", TestHelpers.UniqueEmail(), "abcdef", "abcdef"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Instructor_registration_creates_instructor_who_can_login()
    {
        var email = TestHelpers.UniqueEmail("teacher");
        var response = await _client.PostAsJsonAsync("/api/auth/register-instructor",
            new RegisterInstructorRequest("Teacher", email, "Pass@123", "Pass@123"));
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var auth = await _client.LoginAsync(email, "Pass@123");
        Assert.Equal(["Instructor"], auth.User.Roles);
    }

    [Fact]
    public async Task Refresh_token_rotates_and_cannot_be_reused()
    {
        var auth = await _client.LoginAsync(TestHelpers.AdminEmail, TestHelpers.AdminPassword);

        var first = await _client.PostAsJsonAsync("/api/auth/refresh", new RefreshTokenRequest(auth.RefreshToken));
        first.EnsureSuccessStatusCode();

        var reused = await _client.PostAsJsonAsync("/api/auth/refresh", new RefreshTokenRequest(auth.RefreshToken));
        Assert.Equal(HttpStatusCode.Unauthorized, reused.StatusCode);
    }

    [Fact]
    public async Task Logout_revokes_refresh_token()
    {
        var auth = await _client.LoginAsync(TestHelpers.AdminEmail, TestHelpers.AdminPassword);
        await _client.PostAsJsonAsync("/api/auth/logout", new RefreshTokenRequest(auth.RefreshToken));

        var response = await _client.PostAsJsonAsync("/api/auth/refresh", new RefreshTokenRequest(auth.RefreshToken));
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Me_requires_authentication()
    {
        var response = await factory.CreateClient().GetAsync("/api/auth/me");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BFCAI.Application.Features.Auth;
using BFCAI.Application.Features.Users;

namespace BFCAI.Api.IntegrationTests;

public class UsersTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Students_cannot_access_admin_users()
    {
        var client = factory.CreateClient();
        var email = TestHelpers.UniqueEmail();
        var auth = await (await client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("S", email, "Pass@123", "Pass@123"))).Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.AccessToken);

        var response = await client.GetAsync("/api/admin/users");
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Admin_creates_user_with_role_and_user_type()
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail("inst");

        var created = await (await admin.PostAsJsonAsync("/api/admin/users",
                new UserRequest(email, email, "Instructor", "Inst Name", "Pass@123")))
            .Content.ReadFromJsonAsync<UserDto>();
        Assert.Equal("Instructor", created!.Role);

        var instructors = await admin.GetFromJsonAsync<List<UserListItemDto>>("/api/admin/users?filter=Instructor");
        Assert.Contains(instructors!, u => u.Email == email);
    }

    [Fact]
    public async Task Changing_role_replaces_roles_and_user_type()
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail();
        var created = await (await admin.PostAsJsonAsync("/api/admin/users",
            new UserRequest(email, email, "Student", null, "Pass@123"))).Content.ReadFromJsonAsync<UserDto>();

        var updated = await (await admin.PutAsJsonAsync($"/api/admin/users/{created!.Id}",
            new UserRequest(email, email, "Instructor", null, null))).Content.ReadFromJsonAsync<UserDto>();

        Assert.Equal("Instructor", updated!.Role);
        var students = await admin.GetFromJsonAsync<List<UserListItemDto>>("/api/admin/users?filter=Student");
        Assert.DoesNotContain(students!, u => u.Id == created.Id);
    }

    [Fact]
    public async Task Locked_user_cannot_login_until_unlocked()
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail();
        var created = await (await admin.PostAsJsonAsync("/api/admin/users",
            new UserRequest(email, email, "Student", null, "Pass@123"))).Content.ReadFromJsonAsync<UserDto>();

        await admin.PostAsync($"/api/admin/users/{created!.Id}/lock", null);
        var list = await admin.GetFromJsonAsync<List<UserListItemDto>>("/api/admin/users?filter=Student");
        Assert.True(list!.Single(u => u.Id == created.Id).IsLockedOut);

        var login = await factory.CreateClient().PostAsJsonAsync("/api/auth/login", new LoginRequest(email, "Pass@123"));
        Assert.Equal(HttpStatusCode.Unauthorized, login.StatusCode);

        await admin.PostAsync($"/api/admin/users/{created.Id}/unlock", null);
        await factory.CreateClient().LoginAsync(email, "Pass@123");
    }

    [Fact]
    public async Task Admin_deletes_user()
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail();
        var created = await (await admin.PostAsJsonAsync("/api/admin/users",
            new UserRequest(email, email, "Student", null, "Pass@123"))).Content.ReadFromJsonAsync<UserDto>();

        var response = await admin.DeleteAsync($"/api/admin/users/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await admin.GetAsync($"/api/admin/users/{created.Id}")).StatusCode);
    }
}

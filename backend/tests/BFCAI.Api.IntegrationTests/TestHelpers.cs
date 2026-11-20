using System.Net.Http.Headers;
using System.Net.Http.Json;
using BFCAI.Application.Features.Auth;

namespace BFCAI.Api.IntegrationTests;

public static class TestHelpers
{
    public const string AdminEmail = "Admin@gmail.com";
    public const string AdminPassword = "Admin@123";

    public static async Task<AuthResponse> LoginAsync(this HttpClient client, string email, string password)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginRequest(email, password));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AuthResponse>())!;
    }

    public static async Task<HttpClient> AuthenticatedAsync(this ApiFactory factory, string email, string password)
    {
        var client = factory.CreateClient();
        var auth = await client.LoginAsync(email, password);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
        return client;
    }

    public static Task<HttpClient> AsAdminAsync(this ApiFactory factory) => factory.AuthenticatedAsync(AdminEmail, AdminPassword);

    public static string UniqueEmail(string prefix = "user") => $"{prefix}-{Guid.NewGuid():N}@test.com";
}

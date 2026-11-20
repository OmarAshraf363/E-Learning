using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BFCAI.Application.Features.Account;
using BFCAI.Application.Features.Auth;

namespace BFCAI.Api.IntegrationTests;

public class AccountTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private async Task<(HttpClient Client, string Email)> NewStudentAsync()
    {
        var client = factory.CreateClient();
        var email = TestHelpers.UniqueEmail();
        var auth = await (await client.PostAsJsonAsync("/api/auth/register",
            new RegisterRequest("S", email, "Pass@123", "Pass@123"))).Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.AccessToken);
        return (client, email);
    }

    [Fact]
    public async Task Update_profile_with_picture()
    {
        var (client, _) = await NewStudentAsync();
        using var form = new MultipartFormDataContent
        {
            { new StringContent("0100000000"), "PhoneNumber" },
            { new StringContent("CS student"), "JopDescription" },
            { new ByteArrayContent([1, 2, 3]), "ProfilePicture", "me.png" },
        };

        var response = await client.PutAsync("/api/account/profile", form);
        response.EnsureSuccessStatusCode();
        var profile = await response.Content.ReadFromJsonAsync<ProfileDto>();

        Assert.Equal("CS student", profile!.JopDescription);
        Assert.StartsWith("/Images/profile_pics/", profile.Picture);
    }

    [Fact]
    public async Task Profile_picture_must_be_an_image()
    {
        var (client, _) = await NewStudentAsync();
        using var form = new MultipartFormDataContent { { new ByteArrayContent([1]), "ProfilePicture", "virus.exe" } };

        var response = await client.PutAsync("/api/account/profile", form);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Change_password_then_login_with_new_one()
    {
        var (client, email) = await NewStudentAsync();
        var response = await client.PutAsJsonAsync("/api/account/password",
            new ChangePasswordRequest("Pass@123", "NewPass@456", "NewPass@456"));
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        await factory.CreateClient().LoginAsync(email, "NewPass@456");
    }
}

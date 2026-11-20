using BFCAI.Api.Infrastructure;
using BFCAI.Application.Features.Account;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Auth;

/// <summary>Current user's profile (legacy Identity/Account/Manage pages).</summary>
[ApiController]
[Route("api/account")]
[Authorize]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpGet("profile")]
    public Task<ProfileDto> GetProfile() => service.GetProfileAsync();

    public record UpdateProfileForm(string? PhoneNumber, string? JopDescription, IFormFile? ProfilePicture);

    [HttpPut("profile")]
    public async Task<ProfileDto> UpdateProfile([FromForm] UpdateProfileForm form,
        [FromServices] IValidator<UpdateProfileRequest> validator)
    {
        var request = new UpdateProfileRequest(form.PhoneNumber, form.JopDescription, form.ProfilePicture.ToFileUpload());
        await validator.ValidateAndThrowAsync(request);
        return await service.UpdateProfileAsync(request);
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        await service.ChangePasswordAsync(request);
        return NoContent();
    }
}

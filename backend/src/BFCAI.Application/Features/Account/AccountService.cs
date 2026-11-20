using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BFCAI.Application.Features.Account;

public interface IAccountService
{
    Task<ProfileDto> GetProfileAsync();
    Task<ProfileDto> UpdateProfileAsync(UpdateProfileRequest request);
    Task ChangePasswordAsync(ChangePasswordRequest request);
}

public class AccountService(
    UserManager<ApplicationUser> userManager,
    ICurrentUser currentUser,
    IFileStorage fileStorage) : IAccountService
{
    private const string ProfilePicturesFolder = "Images/profile_pics";

    public async Task<ProfileDto> GetProfileAsync() => ToDto(await GetUserAsync());

    public async Task<ProfileDto> UpdateProfileAsync(UpdateProfileRequest request)
    {
        var user = await GetUserAsync();

        if (request.PhoneNumber != user.PhoneNumber)
            (await userManager.SetPhoneNumberAsync(user, request.PhoneNumber)).ThrowIfFailed(nameof(request.PhoneNumber));

        if (request.ProfilePicture is not null)
        {
            // Legacy: wwwroot/Images/profile_pics/{userId}_{fileName}
            var fileName = await fileStorage.SaveAsync(request.ProfilePicture, ProfilePicturesFolder,
                $"{user.Id}_{Path.GetFileName(request.ProfilePicture.FileName)}");
            var newPicture = $"/{ProfilePicturesFolder}/{fileName}";

            if (user.Picture != newPicture) await fileStorage.DeleteAsync(user.Picture);
            user.Picture = newPicture;
        }

        user.JopDescription = request.JopDescription ?? string.Empty;
        (await userManager.UpdateAsync(user)).ThrowIfFailed();

        return ToDto(user);
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await GetUserAsync();
        (await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword))
            .ThrowIfFailed(nameof(request.NewPassword));
    }

    private async Task<ApplicationUser> GetUserAsync()
        => await userManager.FindByIdAsync(currentUser.RequiredUserId) ?? throw new UnauthorizedException();

    private static ProfileDto ToDto(ApplicationUser user)
        => new(user.UserName ?? string.Empty, user.PhoneNumber, user.JopDescription, user.Picture);
}

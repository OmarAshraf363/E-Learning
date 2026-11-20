using BFCAI.Application.Common.Models;

namespace BFCAI.Application.Features.Account;

/// <summary>Legacy Identity/Account/Manage/Index page.</summary>
public record ProfileDto(string UserName, string? PhoneNumber, string JopDescription, string? Picture);

public record UpdateProfileRequest(string? PhoneNumber, string? JopDescription, FileUpload? ProfilePicture);

public record ChangePasswordRequest(string OldPassword, string NewPassword, string ConfirmPassword);

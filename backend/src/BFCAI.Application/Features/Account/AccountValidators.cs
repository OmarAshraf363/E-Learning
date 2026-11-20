using BFCAI.Application.Common.Models;
using FluentValidation;

namespace BFCAI.Application.Features.Account;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.PhoneNumber).Matches(@"^[0-9+\-\s()]*$").WithMessage("The Phone number field is not a valid phone number.");
        RuleFor(x => x.ProfilePicture!)
            .Must(f => FileRules.HasAllowedExtension(f, FileRules.ImageExtensions))
            .When(x => x.ProfilePicture is not null)
            .WithMessage("Only image files are allowed.");
    }
}

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().Length(6, 100);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword)
            .WithMessage("The new password and confirmation password do not match.");
    }
}

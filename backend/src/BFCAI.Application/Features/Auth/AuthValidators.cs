using FluentValidation;

namespace BFCAI.Application.Features.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

// Same rules as the legacy InputModel / InstructorVM data annotations.
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().Length(6, 100);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
            .WithMessage("The password and confirmation password do not match.");
    }
}

public class RegisterInstructorRequestValidator : AbstractValidator<RegisterInstructorRequest>
{
    public RegisterInstructorRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().Length(6, 100);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
            .WithMessage("The password and confirmation password do not match.");
    }
}

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator() => RuleFor(x => x.RefreshToken).NotEmpty();
}

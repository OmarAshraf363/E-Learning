using BFCAI.Domain.Constants;
using FluentValidation;

namespace BFCAI.Application.Features.Users;

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Role).NotEmpty().Must(r => Roles.All.Contains(r)).WithMessage("Please select a valid role.");
    }
}

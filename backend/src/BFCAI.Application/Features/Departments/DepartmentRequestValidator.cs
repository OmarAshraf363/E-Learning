using BFCAI.Application.Common;
using FluentValidation;

namespace BFCAI.Application.Features.Departments;

public class DepartmentRequestValidator : AbstractValidator<DepartmentRequest>
{
    public DepartmentRequestValidator()
    {
        RuleFor(x => x.DepartmentName).NotEmpty();
        RuleFor(x => x.DepartmentDescription).NotEmpty().WithName("Description");
        RuleFor(x => x.Cover).MustBeImage();
    }
}

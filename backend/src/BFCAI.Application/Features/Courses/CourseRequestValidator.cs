using BFCAI.Application.Common;
using FluentValidation;

namespace BFCAI.Application.Features.Courses;

/// <summary>Same rules as the legacy CourseVM data annotations.</summary>
public class CourseRequestValidator : AbstractValidator<CourseRequest>
{
    public CourseRequestValidator()
    {
        RuleFor(x => x.CourseName).NotEmpty().WithMessage("Course name is required.")
            .MaximumLength(100).WithMessage("Course name cannot be longer than 100 characters.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Course description is required.")
            .MaximumLength(1000).WithMessage("Description cannot be longer than 1000 characters.");
        RuleFor(x => x.Credits).InclusiveBetween(1, 50).WithMessage("Credits must be between 1 and 50.");
        RuleFor(x => x.InstructorId).NotEmpty().WithMessage("Instructor is required.");
        RuleFor(x => x.DepartmentId).NotNull().WithMessage("Department is required.");
        RuleFor(x => x.DemoVideoUrl).NotEmpty().WithMessage("The DemoVideoUrl field is required.")
            .Must(u => Uri.TryCreate(u, UriKind.Absolute, out _)).WithMessage("Invalid URL format.");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value.");
        RuleFor(x => x.StartDate).NotNull().WithMessage("Start date is required.");
        RuleFor(x => x.EndDate).NotNull().WithMessage("End date is required.");
        RuleFor(x => x.ImgCover).MustBeImage();
    }
}

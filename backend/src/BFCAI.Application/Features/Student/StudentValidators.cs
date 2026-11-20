using FluentValidation;

namespace BFCAI.Application.Features.Student;

public class FeedbackRequestValidator : AbstractValidator<FeedbackRequest>
{
    public FeedbackRequestValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Rating).InclusiveBetween(1, 5).When(x => x.Rating is not null);
        RuleFor(x => x).Must(x => x.CourseId is not null || x.CourseCurriculumId is not null)
            .WithMessage("Feedback must target a course or a curriculum section.");
    }
}

public class SubmitAssignmentRequestValidator : AbstractValidator<SubmitAssignmentRequest>
{
    public SubmitAssignmentRequestValidator() => RuleFor(x => x.Content).NotEmpty();
}

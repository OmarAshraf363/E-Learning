using FluentValidation;

namespace BFCAI.Application.Features.Schedules;

public class ScheduleRequestValidator : AbstractValidator<ScheduleRequest>
{
    public static readonly string[] Days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

    public ScheduleRequestValidator()
    {
        RuleFor(x => x.StartTime).Must(BeTime).WithMessage("Please enter a valid start time.");
        RuleFor(x => x.EndTime).Must(BeTime).WithMessage("Please enter a valid end time.");
        RuleFor(x => x.DayOfWeek).Must(d => Days.Contains(d)).WithMessage("Please select a day.");
        RuleFor(x => x.CourseId).GreaterThan(0).WithMessage("Please select a course.");
    }

    private static bool BeTime(string value) => TimeOnly.TryParse(value, out _);
}

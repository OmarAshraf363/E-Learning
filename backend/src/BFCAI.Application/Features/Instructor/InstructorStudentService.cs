using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Entities;
using FluentValidation;

namespace BFCAI.Application.Features.Instructor;

public record CourseStudentDto(string Id, string FullName, string? Email, string? Picture, decimal? Gpa);

public record StudentFeedbackRequest(string StudentId, string Content);

public class StudentFeedbackRequestValidator : AbstractValidator<StudentFeedbackRequest>
{
    public StudentFeedbackRequestValidator()
    {
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
    }
}

/// <summary>Legacy Instructor/StudentController: students of a course + feedback to a student.</summary>
public interface IInstructorStudentService
{
    Task<List<CourseStudentDto>> GetStudentsAsync(int courseId);
    Task AddFeedbackAsync(StudentFeedbackRequest request);
}

public class InstructorStudentService(IUnitOfWork unitOfWork, InstructorAccess access) : IInstructorStudentService
{
    /// <summary>Technical fix: legacy listed the students of any course id, not only the instructor's.</summary>
    public async Task<List<CourseStudentDto>> GetStudentsAsync(int courseId)
    {
        await access.EnsureOwnsCourseAsync(courseId);
        return await unitOfWork.Repository<Enrollment>().SelectAsync(
            e => new CourseStudentDto(e.Student!.Id, e.Student.FullName, e.Student.Email, e.Student.Picture, e.Student.Gpa),
            e => e.CourseID == courseId && e.Student != null);
    }

    /// <summary>The provider is the current instructor (legacy trusted a hidden form field).</summary>
    public async Task AddFeedbackAsync(StudentFeedbackRequest request)
    {
        await unitOfWork.Repository<Feedback>().AddAsync(new Feedback
        {
            Content = request.Content,
            TargetStudentUserId = request.StudentId,
            ProviderUserId = access.InstructorId,
            FeedbackDate = DateTime.Now,
        });
        await unitOfWork.SaveChangesAsync();
    }
}

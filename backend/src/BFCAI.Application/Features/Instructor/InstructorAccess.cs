using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Instructor;

/// <summary>
/// Ownership checks for the instructor area. Technical / security fix: the legacy instructor
/// actions edited or deleted any course, section, exam or assignment by id.
/// </summary>
public class InstructorAccess(IUnitOfWork unitOfWork, ICurrentUser currentUser)
{
    public string InstructorId => currentUser.RequiredUserId;

    public async Task EnsureOwnsCourseAsync(int courseId)
    {
        var instructorId = InstructorId;
        if (!await unitOfWork.Repository<Course>().AnyAsync(c => c.CourseID == courseId))
            throw new NotFoundException(nameof(Course), courseId);
        if (!await unitOfWork.Repository<Course>().AnyAsync(c => c.CourseID == courseId && c.InstructorId == instructorId))
            throw new ForbiddenException();
    }

    /// <summary>Returns the section's course id.</summary>
    public async Task<int> EnsureOwnsCurriculumAsync(int curriculumId)
    {
        var courseId = await unitOfWork.Repository<CourseCurriculum>()
                           .SelectOneAsync(c => c.CourseCurriculumID == curriculumId, c => (int?)c.CourseID)
                       ?? throw new NotFoundException("Curriculum", curriculumId);
        await EnsureOwnsCourseAsync(courseId);
        return courseId;
    }

    public async Task EnsureOwnsExamAsync(int examId)
    {
        var instructorId = InstructorId;
        var exam = await unitOfWork.Repository<Exam>().SelectOneAsync(e => e.ExamID == examId,
                       e => new { e.InstructorId, CourseInstructor = e.Course!.InstructorId })
                   ?? throw new NotFoundException(nameof(Exam), examId);
        if (exam.InstructorId != instructorId && exam.CourseInstructor != instructorId)
            throw new ForbiddenException();
    }
}

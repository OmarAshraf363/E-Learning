using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.Courses;
using BFCAI.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace BFCAI.Application.Features.Instructor;

/// <summary>Legacy InstructorDashboardViewModel.</summary>
public record InstructorDashboardDto(int TotalCourses, int TotalExams, int TotalQuestions, int TotalStudents);

public record InstructorCourseRowDto(int Id, string Name, int StudentsEnrolled);

public record InstructorCourseLookupsDto(List<LookupItem<int>> Departments, List<LookupItem<int>> Tracks);

/// <summary>Legacy Instructor/Dashboard + InstructorCourse controller (reuses the admin course logic).</summary>
public interface IInstructorCourseService
{
    Task<InstructorDashboardDto> GetDashboardAsync();
    Task<PagedResult<InstructorCourseRowDto>> GetMyCoursesAsync(PaginationParams pagination);
    Task<List<LookupItem<int>>> GetMyCourseLookupAsync();
    Task<InstructorCourseLookupsDto> GetFormLookupsAsync();
    Task<CourseFormDto> GetFormAsync(int id);
    Task<CourseFormDto> CreateAsync(CourseRequest request);
    Task<CourseFormDto> UpdateAsync(int id, CourseRequest request);
}

public class InstructorCourseService(IUnitOfWork unitOfWork, InstructorAccess access, ICourseManagementService courses)
    : IInstructorCourseService
{
    public async Task<InstructorDashboardDto> GetDashboardAsync()
    {
        var me = access.InstructorId;
        var studentIds = await unitOfWork.Repository<Enrollment>().SelectAsync(e => e.StudentId, e => e.Course!.InstructorId == me);

        return new InstructorDashboardDto(
            await unitOfWork.Repository<Course>().CountAsync(c => c.InstructorId == me),
            await unitOfWork.Repository<Exam>().CountAsync(e => e.InstructorId == me),
            await unitOfWork.Repository<Question>().CountAsync(q => q.Exam!.InstructorId == me),
            studentIds.Distinct().Count());
    }

    public Task<PagedResult<InstructorCourseRowDto>> GetMyCoursesAsync(PaginationParams pagination)
    {
        var me = access.InstructorId;
        return unitOfWork.Repository<Course>().GetPagedAsync(
            c => new InstructorCourseRowDto(c.CourseID, c.CourseName, c.Enrollments.Count),
            pagination, c => c.InstructorId == me, q => q.OrderBy(c => c.CourseID));
    }

    public Task<List<LookupItem<int>>> GetMyCourseLookupAsync()
    {
        var me = access.InstructorId;
        return unitOfWork.Repository<Course>().SelectAsync(c => new LookupItem<int>(c.CourseID, c.CourseName), c => c.InstructorId == me);
    }

    /// <summary>
    /// Legacy: departments the instructor belongs to and tracks where they already teach.
    /// When the instructor is not assigned to any department, all departments are offered
    /// (department is required, so the legacy form could not be submitted in that case).
    /// </summary>
    public async Task<InstructorCourseLookupsDto> GetFormLookupsAsync()
    {
        var me = access.InstructorId;
        var departments = unitOfWork.Repository<Department>();

        var mine = await departments.SelectAsync(d => new LookupItem<int>(d.DepartmentID, d.DepartmentName), d => d.Users.Any(u => u.Id == me));
        if (mine.Count == 0)
            mine = await departments.SelectAsync(d => new LookupItem<int>(d.DepartmentID, d.DepartmentName));

        var tracks = await unitOfWork.Repository<Track>().SelectAsync(t => new LookupItem<int>(t.Id, t.Name),
            t => t.Courses.Any(c => c.InstructorId == me));

        return new InstructorCourseLookupsDto(mine, tracks);
    }

    public async Task<CourseFormDto> GetFormAsync(int id)
    {
        await access.EnsureOwnsCourseAsync(id);
        return await courses.GetFormAsync(id);
    }

    /// <summary>The instructor is always the current user (legacy trusted a hidden form field).</summary>
    public async Task<CourseFormDto> CreateAsync(CourseRequest request)
    {
        await EnsureDepartmentAllowedAsync(request.DepartmentId);
        return await courses.CreateAsync(request with { InstructorId = access.InstructorId });
    }

    public async Task<CourseFormDto> UpdateAsync(int id, CourseRequest request)
    {
        await access.EnsureOwnsCourseAsync(id);
        await EnsureDepartmentAllowedAsync(request.DepartmentId);
        return await courses.UpdateAsync(id, request with { InstructorId = access.InstructorId });
    }

    /// <summary>An instructor linked to a department creates courses in that department only (same list as the form).</summary>
    private async Task EnsureDepartmentAllowedAsync(int? departmentId)
    {
        var me = access.InstructorId;
        var myDepartmentId = await unitOfWork.Repository<ApplicationUser>().SelectOneAsync(u => u.Id == me, u => u.DepartmentId);
        if (myDepartmentId is not null && departmentId != myDepartmentId)
            throw new ValidationException([new ValidationFailure(nameof(CourseRequest.DepartmentId), "Please select your department.")]);
    }
}

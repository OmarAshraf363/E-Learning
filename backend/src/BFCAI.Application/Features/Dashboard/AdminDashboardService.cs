using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Domain.Constants;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Dashboard;

public record DashboardCourseDto(int Id, string Name, string? DepartmentName);

public record PopularCourseDto(string Name, int EnrollmentCount);

public record DashboardEventDto(string Name, DateTime Date);

/// <summary>Legacy Admin/DashBoard/Index.</summary>
public record AdminDashboardDto(
    int TotalStudents,
    int TotalInstructors,
    int TotalCourses,
    int TotalDepartments,
    PagedResult<DashboardCourseDto> Courses,
    PopularCourseDto? MostPopularCourse,
    List<DashboardEventDto> Events);

public interface IAdminDashboardService
{
    Task<AdminDashboardDto> GetAsync(PaginationParams pagination);
}

public class AdminDashboardService(IUnitOfWork unitOfWork) : IAdminDashboardService
{
    public async Task<AdminDashboardDto> GetAsync(PaginationParams pagination)
    {
        var users = unitOfWork.Repository<ApplicationUser>();
        var courses = unitOfWork.Repository<Course>();

        var page = await courses.GetPagedAsync(
            c => new DashboardCourseDto(c.CourseID, c.CourseName, c.Department != null ? c.Department.DepartmentName : null),
            pagination,
            orderBy: q => q.OrderByDescending(c => c.CourseID));

        // Technical fix: legacy showed the size of the first page as "Total Courses" and never filled
        // "Most Popular Course" (ViewBag was not set). Both now use real values.
        var popular = (await courses.SelectAsync(
                c => new PopularCourseDto(c.CourseName, c.Enrollments.Count),
                orderBy: q => q.OrderByDescending(c => c.Enrollments.Count),
                take: 1))
            .FirstOrDefault(p => p.EnrollmentCount > 0);

        return new AdminDashboardDto(
            await users.CountAsync(u => u.UserType == Roles.Student),
            await users.CountAsync(u => u.UserType == Roles.Instructor),
            page.TotalCount,
            await unitOfWork.Repository<Department>().CountAsync(),
            page,
            popular,
            await unitOfWork.Repository<Event>().SelectAsync(e => new DashboardEventDto(e.Name, e.Date)));
    }
}

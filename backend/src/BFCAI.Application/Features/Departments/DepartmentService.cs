using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.Courses;
using BFCAI.Application.Features.Schedules;
using BFCAI.Domain.Constants;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Departments;

/// <summary>Legacy Admin/DepartmentController.</summary>
public interface IDepartmentService
{
    Task<List<DepartmentListItemDto>> GetAllAsync();
    Task<DepartmentDto> GetAsync(int id);
    Task<DepartmentDetailsDto> GetDetailsAsync(int id);
    Task<List<AdminCourseRowDto>> GetCoursesAsync(int id);
    Task<DepartmentDto> CreateAsync(DepartmentRequest request);
    Task<DepartmentDto> UpdateAsync(int id, DepartmentRequest request);
    Task DeleteAsync(int id);
}

public class DepartmentService(IUnitOfWork unitOfWork, IFileStorage files, ICacheService cache) : IDepartmentService
{
    private IGenericRepository<Department> Departments => unitOfWork.Repository<Department>();

    public Task<List<DepartmentListItemDto>> GetAllAsync()
        => Departments.SelectAsync(d => new DepartmentListItemDto(
            d.DepartmentID, d.DepartmentName, d.DepartmentDescription, FilePaths.Cover(d.Cover),
            d.Courses.Sum(c => c.Credits)));

    public async Task<DepartmentDto> GetAsync(int id)
        => await Departments.SelectOneAsync(d => d.DepartmentID == id,
               d => new DepartmentDto(d.DepartmentID, d.DepartmentName, d.DepartmentDescription, FilePaths.Cover(d.Cover)))
           ?? throw new NotFoundException(nameof(Department), id);

    public async Task<DepartmentDetailsDto> GetDetailsAsync(int id)
        => await Departments.SelectOneAsync(d => d.DepartmentID == id, d => new DepartmentDetailsDto(
               d.DepartmentID,
               d.DepartmentName,
               d.Users.Count(u => u.UserType == Roles.Student),
               d.Users.Count(u => u.UserType == Roles.Instructor),
               d.Courses.Sum(c => c.Credits),
               d.Courses.Count,
               d.Courses.Select(c => new DepartmentCourseDto(c.CourseID, c.CourseName, c.Credits)).ToList(),
               d.Users.Where(u => u.UserType == Roles.Instructor)
                   .Select(u => new DepartmentUserDto(u.Id, u.UserName!, u.Email!)).ToList(),
               d.ClassSchedules.Select(s => new ScheduleDto(s.ClassScheduleId, s.StartTime, s.EndTime, s.DayOfWeek,
                   s.Course.CourseName, s.Instructor != null ? s.Instructor.FullName : null,
                   s.Room != null ? s.Room.Name : null)).ToList(),
               d.Events.Select(e => new DepartmentEventDto(e.EventID, e.Name, e.Date)).ToList()))
           ?? throw new NotFoundException(nameof(Department), id);

    public Task<List<AdminCourseRowDto>> GetCoursesAsync(int id)
        => unitOfWork.Repository<Course>().SelectAsync(AdminCourseRowDto.Projection, c => c.DepartmentId == id);

    public async Task<DepartmentDto> CreateAsync(DepartmentRequest request)
    {
        if (request.Cover is null) Validation.Fail(nameof(request.Cover), "Please upload an image.");

        var department = new Department
        {
            DepartmentName = request.DepartmentName,
            DepartmentDescription = request.DepartmentDescription,
            Cover = await SaveCoverAsync(request.Cover!),
        };
        await Departments.AddAsync(department);
        await unitOfWork.SaveChangesAsync();
        await cache.RemoveAsync(CacheKeys.Catalog);

        return await GetAsync(department.DepartmentID);
    }

    public async Task<DepartmentDto> UpdateAsync(int id, DepartmentRequest request)
    {
        var department = await Departments.GetOneAsync(d => d.DepartmentID == id, tracked: true)
                         ?? throw new NotFoundException(nameof(Department), id);

        department.DepartmentName = request.DepartmentName;
        department.DepartmentDescription = request.DepartmentDescription;
        if (request.Cover is not null)
        {
            // Technical fix: the legacy form cleared the cover when no new image was uploaded.
            var oldCover = department.Cover;
            department.Cover = await SaveCoverAsync(request.Cover);
            await files.DeleteAsync($"{FilePaths.CoversFolder}/{oldCover}");
        }

        await unitOfWork.SaveChangesAsync();
        await cache.RemoveAsync(CacheKeys.Catalog);
        return await GetAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var department = await Departments.GetOneAsync(d => d.DepartmentID == id, tracked: true)
                         ?? throw new NotFoundException(nameof(Department), id);

        Departments.Delete(department);
        await unitOfWork.SaveChangesAsync();
        await cache.RemoveAsync(CacheKeys.Catalog);
    }

    private Task<string> SaveCoverAsync(FileUpload cover)
        => files.SaveAsync(cover, FilePaths.CoversFolder, FilePaths.NewCoverName(cover.FileName));
}

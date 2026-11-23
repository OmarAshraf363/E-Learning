using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Domain.Constants;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Lookups;

/// <summary>Dropdown data used by the admin forms (departments, tracks, instructors...).</summary>
public interface ILookupService
{
    Task<List<LookupItem<int>>> DepartmentsAsync();
    Task<List<LookupItem<int>>> TracksAsync();
    Task<List<LookupItem<int>>> CoursesAsync();
    Task<List<LookupItem<int>>> RoomsAsync();
    Task<List<LookupItem<int>>> AcademicYearsAsync();
    /// <summary>Users whose UserType is Instructor (shows FullName; used by the course and schedule forms).</summary>
    Task<List<LookupItem<string>>> InstructorsAsync();
}

public class LookupService(IUnitOfWork unitOfWork) : ILookupService
{
    public Task<List<LookupItem<int>>> DepartmentsAsync()
        => unitOfWork.Repository<Department>().SelectAsync(d => new LookupItem<int>(d.DepartmentID, d.DepartmentName));

    public Task<List<LookupItem<int>>> TracksAsync()
        => unitOfWork.Repository<Track>().SelectAsync(t => new LookupItem<int>(t.Id, t.Name));

    public Task<List<LookupItem<int>>> CoursesAsync()
        => unitOfWork.Repository<Course>().SelectAsync(c => new LookupItem<int>(c.CourseID, c.CourseName));

    public Task<List<LookupItem<int>>> RoomsAsync()
        => unitOfWork.Repository<Room>().SelectAsync(r => new LookupItem<int>(r.RoomID, r.Name));

    public Task<List<LookupItem<int>>> AcademicYearsAsync()
        => unitOfWork.Repository<AcademicYear>().SelectAsync(y => new LookupItem<int>(y.AcademicYearID, y.YearName));

    public Task<List<LookupItem<string>>> InstructorsAsync()
        => unitOfWork.Repository<ApplicationUser>().SelectAsync(
            u => new LookupItem<string>(u.Id, u.FullName),
            u => u.UserType == Roles.Instructor);
}

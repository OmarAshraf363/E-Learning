using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Schedules;

/// <summary>Legacy Admin/ScheduleController.</summary>
public interface IScheduleService
{
    Task<List<ScheduleDto>> GetAllAsync();
    Task<ScheduleFormDto> GetAsync(int id);
    Task<ScheduleFormDto> CreateAsync(ScheduleRequest request);
    Task<ScheduleFormDto> UpdateAsync(int id, ScheduleRequest request);
    Task DeleteAsync(int id);
}

public class ScheduleService(IUnitOfWork unitOfWork) : IScheduleService
{
    private IGenericRepository<ClassSchedule> Schedules => unitOfWork.Repository<ClassSchedule>();

    public Task<List<ScheduleDto>> GetAllAsync()
        => Schedules.SelectAsync(s => new ScheduleDto(s.ClassScheduleId, s.StartTime, s.EndTime, s.DayOfWeek,
            s.Course.CourseName, s.Instructor != null ? s.Instructor.FullName : null, s.Room != null ? s.Room.Name : null));

    public async Task<ScheduleFormDto> GetAsync(int id)
    {
        var s = await Schedules.GetOneAsync(x => x.ClassScheduleId == id) ?? throw new NotFoundException("Schedule", id);
        return new ScheduleFormDto(s.ClassScheduleId, s.StartTime.ToString("HH:mm"), s.EndTime.ToString("HH:mm"),
            s.DayOfWeek, s.CourseId, s.InstructorId, s.RoomId, s.DepartmentId, s.AcadmicYearId);
    }

    public async Task<ScheduleFormDto> CreateAsync(ScheduleRequest request)
    {
        var schedule = new ClassSchedule();
        Apply(schedule, request);
        await Schedules.AddAsync(schedule);
        await unitOfWork.SaveChangesAsync();
        return await GetAsync(schedule.ClassScheduleId);
    }

    public async Task<ScheduleFormDto> UpdateAsync(int id, ScheduleRequest request)
    {
        var schedule = await Schedules.GetOneAsync(x => x.ClassScheduleId == id, tracked: true)
                       ?? throw new NotFoundException("Schedule", id);
        Apply(schedule, request);
        await unitOfWork.SaveChangesAsync();
        return await GetAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var schedule = await Schedules.GetOneAsync(x => x.ClassScheduleId == id, tracked: true)
                       ?? throw new NotFoundException("Schedule", id);
        Schedules.Delete(schedule);
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Legacy &lt;input type="time"&gt; bound to DateTime = today's date + the selected time.
    /// Technical fix: InstructorId is the user's string id (legacy int.Parse failed on GUID ids).
    /// </summary>
    private static void Apply(ClassSchedule schedule, ScheduleRequest request)
    {
        schedule.StartTime = DateTime.Today.Add(TimeOnly.Parse(request.StartTime).ToTimeSpan());
        schedule.EndTime = DateTime.Today.Add(TimeOnly.Parse(request.EndTime).ToTimeSpan());
        schedule.DayOfWeek = request.DayOfWeek;
        schedule.CourseId = request.CourseId;
        schedule.InstructorId = string.IsNullOrEmpty(request.InstructorId) ? null : request.InstructorId;
        schedule.RoomId = request.RoomId is > 0 ? request.RoomId : null;
        schedule.DepartmentId = request.DepartmentId;
        schedule.AcadmicYearId = request.AcadmicYearId;
    }
}

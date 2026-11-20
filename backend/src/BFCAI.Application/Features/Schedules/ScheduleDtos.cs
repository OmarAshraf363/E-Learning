namespace BFCAI.Application.Features.Schedules;

/// <summary>Row of the class schedule table (admin schedule page and department details).</summary>
public record ScheduleDto(
    int Id,
    DateTime StartTime,
    DateTime EndTime,
    string DayOfWeek,
    string CourseName,
    string? InstructorName,
    string? RoomName);

/// <summary>Schedule edit form. Times are "HH:mm".</summary>
public record ScheduleFormDto(
    int Id,
    string StartTime,
    string EndTime,
    string DayOfWeek,
    int CourseId,
    string? InstructorId,
    int? RoomId,
    int? DepartmentId,
    int? AcadmicYearId);

/// <summary>Create / edit. Times are "HH:mm" (legacy &lt;input type="time"&gt;).</summary>
public record ScheduleRequest(
    string StartTime,
    string EndTime,
    string DayOfWeek,
    int CourseId,
    string? InstructorId,
    int? RoomId,
    int? DepartmentId,
    int? AcadmicYearId);

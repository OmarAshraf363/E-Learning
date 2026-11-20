using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.Schedules;

namespace BFCAI.Application.Features.Departments;

public record DepartmentListItemDto(int Id, string Name, string Description, string? Cover, int TotalHours);

public record DepartmentDto(int Id, string Name, string Description, string? Cover);

/// <summary>Create / edit. Cover is required on create (legacy "Please upload an image.").</summary>
public record DepartmentRequest(string DepartmentName, string DepartmentDescription, FileUpload? Cover);

public record DepartmentCourseDto(int Id, string Name, int Credits);

public record DepartmentUserDto(string Id, string UserName, string Email);

public record DepartmentEventDto(int Id, string Name, DateTime Date);

/// <summary>Legacy Department/Details page.</summary>
public record DepartmentDetailsDto(
    int Id,
    string Name,
    int TotalStudents,
    int TotalInstructors,
    int TotalHours,
    int TotalCourses,
    List<DepartmentCourseDto> Courses,
    List<DepartmentUserDto> Instructors,
    List<ScheduleDto> Schedules,
    List<DepartmentEventDto> Events);

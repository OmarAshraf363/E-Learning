using System.Linq.Expressions;
using BFCAI.Application.Common.Models;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Courses;

/// <summary>Row of the admin courses tables (Course/Index and Department/Courses).</summary>
public record AdminCourseRowDto(int Id, string Name, string? DepartmentName, string? InstructorName)
{
    public static readonly Expression<Func<Course, AdminCourseRowDto>> Projection = c => new AdminCourseRowDto(
        c.CourseID,
        c.CourseName,
        c.Department != null ? c.Department.DepartmentName : null,
        c.Instructor != null ? c.Instructor.UserName : null);
}

/// <summary>Legacy Admin/Course/Details.</summary>
public record AdminCourseDetailsDto(
    int Id,
    string Name,
    string Description,
    int Credits,
    string? InstructorName,
    string? DepartmentName,
    int EnrollmentsCount);

/// <summary>Values of the course form (edit mode).</summary>
public record CourseFormDto(
    int Id,
    string CourseName,
    string Description,
    int Credits,
    decimal? Price,
    int? Rate,
    string? DemoVideoUrl,
    int? NumOfEnrollments,
    DateTime? StartDate,
    DateTime? EndDate,
    string InstructorId,
    int? DepartmentId,
    int? TrackId,
    string? ImgCover,
    List<string> KeyWords,
    List<string> LearningObjectives,
    List<string> TopicsCovered);

/// <summary>
/// Create / edit a course (legacy CourseVM). Cover is required on create.
/// KeyWords / LearningObjectives / TopicsCovered replace the existing lists.
/// </summary>
public record CourseRequest(
    string CourseName,
    string Description,
    int Credits,
    decimal? Price,
    int? Rate,
    string? DemoVideoUrl,
    int? NumOfEnrollments,
    DateTime? StartDate,
    DateTime? EndDate,
    string? InstructorId,
    int? DepartmentId,
    int? TrackId,
    FileUpload? ImgCover,
    List<string>? KeyWords,
    List<string>? LearningObjectives,
    List<string>? TopicsCovered);

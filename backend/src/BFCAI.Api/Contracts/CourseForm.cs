using BFCAI.Api.Infrastructure;
using BFCAI.Application.Features.Courses;

namespace BFCAI.Api.Contracts;

/// <summary>
/// multipart/form-data body of the course form (admin and instructor).
/// Lists are sent as repeated keys (KeyWords=a&amp;KeyWords=b).
/// </summary>
public class CourseForm
{
    public string CourseName { get; set; } = "";
    public string Description { get; set; } = "";
    public int Credits { get; set; }
    public decimal? Price { get; set; }
    public int? Rate { get; set; }
    public string? DemoVideoUrl { get; set; }
    public int? NumOfEnrollments { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? InstructorId { get; set; }
    public int? DepartmentId { get; set; }
    public int? TrackId { get; set; }
    public IFormFile? ImgCover { get; set; }
    public List<string>? KeyWords { get; set; }
    public List<string>? LearningObjectives { get; set; }
    public List<string>? TopicsCovered { get; set; }

    public CourseRequest ToRequest() => new(CourseName, Description, Credits, Price, Rate, DemoVideoUrl,
        NumOfEnrollments, StartDate, EndDate, InstructorId, DepartmentId, TrackId, ImgCover.ToFileUpload(),
        KeyWords, LearningObjectives, TopicsCovered);
}

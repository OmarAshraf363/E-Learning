using System.Linq.Expressions;
using BFCAI.Application.Common;
using BFCAI.Application.Common.Models;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Catalog;

/// <summary>Course card + hover popover (legacy CourseDetailViewModel / PrepareCoursesToViewDetailAsync).</summary>
public record CourseCardDto(
    int Id,
    string Title,
    string? Image,
    string? InstructorName,
    int? Rate,
    decimal? Price,
    string Description,
    int NumOfEnrolments,
    int VideosCount,
    List<string> LearningObjectives)
{
    public static readonly Expression<Func<Course, CourseCardDto>> Projection = c => new CourseCardDto(
        c.CourseID,
        c.CourseName,
        FilePaths.Cover(c.ImgCover),
        c.Instructor != null ? c.Instructor.FullName : null,
        c.Rate,
        c.Price,
        c.Description,
        c.Enrollments.Count,
        c.CourseVideos.Count,
        c.LearningObjectives!.Select(o => o.Objective).ToList());
}

public record DepartmentCardDto(int Id, string Name, string Description, string? Cover);

public record TrackCardDto(int Id, string Title, string Description, string? Cover, int NumOfCourses, int NumOfEnrollments);

public record TestimonialDto(int Id, string? UserName, string? UserPicture, int CourseId, string CourseName, string Content, DateTime Date);

public record PublicEventDto(int Id, DateTime Date, string Name, string Description, string? CreatedBy, string? Department);

/// <summary>Legacy Home/Index (AllModelsVM).</summary>
public record HomeDto(
    List<CourseCardDto> TopCourses,
    List<DepartmentCardDto> TopDepartments,
    List<TrackCardDto> TopTracks,
    PagedResult<CourseCardDto> Courses,
    List<CourseCardDto> Recommendations,
    List<int> EnrolledCourseIds,
    List<TestimonialDto> Testimonials,
    List<PublicEventDto> Events);

/// <summary>Legacy GetByDepTId partial (home category tabs).</summary>
public record DepartmentCoursesDto(string? DepartmentName, string? DepartmentDescription, PagedResult<CourseCardDto> Courses);

/// <summary>Legacy CourseParam (courses page filters).</summary>
public class CourseFilter
{
    public string? Search { get; set; }
    public string? InstructorId { get; set; }
    public int? CategoryId { get; set; }
    public decimal? Price { get; set; }
    public int? Rate { get; set; }
    /// <summary>relevance (default) | price | rate | popularity</summary>
    public string? SortBy { get; set; }
    /// <summary>asc (default) | desc</summary>
    public string? SortOrder { get; set; }
}

public record ReviewSummaryDto(int TotalReviews, int FinalRate, Dictionary<int, int> CountByStars);

public record ReviewDto(int Id, string Content, DateTime CreatedAt, int? Rating, string UserName, string? UserImageUrl);

/// <summary>Legacy Home/Details (DetailsVM).</summary>
public record CourseDetailsDto(
    int Id,
    string CourseName,
    string Description,
    string? DemoVideoUrl,
    string? DemoTitle,
    int? Rate,
    decimal? Price,
    int NumOfFeedbacks,
    int NumOfVideos,
    int NumOfEnrollments,
    string InstructorId,
    string? InstructorName,
    string? InstructorBio,
    string? InstructorImage,
    int? DepartmentId,
    int? TrackId,
    List<string> LearningObjectives,
    List<string> TopicsCovered,
    List<CourseCardDto> RelatedCourses,
    List<CourseCardDto> InstructorCourses,
    ReviewSummaryDto ReviewSummary,
    List<ReviewDto> Reviews);

/// <summary>Legacy Home/Category with an id.</summary>
public record CategoryDto(int Id, string Name, string Description, List<CourseCardDto> Courses, List<string> KeyWords);

/// <summary>Legacy Home/TrackDetails.</summary>
public record PublicTrackDto(
    int Id,
    string Title,
    string Description,
    string? Cover,
    int NumOfCourses,
    int NumOfEnrollments,
    int NumOfCoursesStudentHas,
    List<CourseCardDto> Courses,
    List<CourseCardDto> CoursesStudentDoesNotHave);

/// <summary>Navbar "Categories | Tracks" menu (legacy Categories view component).</summary>
public record NavCategoryDto(int Id, string Name, List<LookupItem<int>> Courses);

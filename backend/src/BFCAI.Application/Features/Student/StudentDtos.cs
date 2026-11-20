using BFCAI.Application.Features.Catalog;

namespace BFCAI.Application.Features.Student;

public record StudentCourseRowDto(int Id, string Name, DateTime? StartDate, DateTime? EndDate);
public record StudentScheduleDto(int CourseId, string CourseName, DateTime StartTime);
public record ReceivedFeedbackDto(int Id, string? ProviderName, string? ProviderPicture, DateTime Date, int? Rating, string Content);
public record RelatedPostDto(int Id, string Content, string? Attachment, DateTime CreatedAt, string? UserName, string? UserPicture, int? CommunityId);
public record SuggestedCommunityDto(int Id, string Name, string Description);

/// <summary>Legacy Student/DashBoard/Index (HomeStudentViewModels).</summary>
public record StudentDashboardDto(
    List<StudentCourseRowDto> Courses,
    List<StudentScheduleDto> Schedule,
    List<ReceivedFeedbackDto> Feedbacks,
    List<CourseCardDto> Recommendations,
    List<RelatedPostDto> Posts,
    List<SuggestedCommunityDto> Communities);

/// <summary>Legacy Student/Courses/Index card (+ the student's progress).</summary>
public record MyCourseDto(int Id, string Name, string? Cover, string? InstructorName, int? Rate, decimal ProgressPercentage);

public record SectionLinkDto(int Id, string Title);
public record CourseFeedbackDto(int Id, string? UserName, string? UserPicture, DateTime Date, int? Rating, string Content, bool IsMine);

/// <summary>Legacy Student/DashBoard/CourseProcess.</summary>
public record CoursePlayerDto(int CourseId, string CourseName, List<SectionLinkDto> Sections, List<CourseFeedbackDto> Feedbacks);

public record PlayerVideoDto(int Id, string Title, string Url);
public record PlayerResourceDto(int Id, string Title, string Url);
public record PlayerAssignmentDto(int Id, string Title, string Url, DateTime DeadLine, bool Submitted);

/// <summary>Legacy Student/DashBoard/GetContent (one curriculum section).</summary>
public record SectionContentDto(
    int Id,
    string Title,
    string Content,
    List<PlayerVideoDto> Videos,
    List<PlayerResourceDto> Resources,
    List<PlayerAssignmentDto> Assignments,
    List<CourseFeedbackDto> Feedbacks,
    bool HasQuiz);

/// <summary>Quiz questions shown to the student (the correct answers are never sent).</summary>
public record QuizQuestionDto(string QuestionText, List<string> Choices);

public record QuickAccessSectionDto(string Title, string Content, List<PlayerVideoDto> Videos, List<PlayerResourceDto> Resources, List<PlayerAssignmentDto> Assignments);

/// <summary>Legacy Student/DashBoard/QuickAccess.</summary>
public record QuickAccessDto(int CourseId, string CourseName, List<QuickAccessSectionDto> Sections);

/// <summary>Course feedback (CourseId) or section feedback (CourseCurriculumId).</summary>
public record FeedbackRequest(int? CourseId, int? CourseCurriculumId, int? Rating, string Content);

public record SubmitAssignmentRequest(string Content);

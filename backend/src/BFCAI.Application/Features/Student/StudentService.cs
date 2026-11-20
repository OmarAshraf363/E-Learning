using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Features.Catalog;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Student;

/// <summary>Legacy Student area (DashBoard, Courses, StudentAssinment controllers).</summary>
public interface IStudentService
{
    Task<StudentDashboardDto> GetDashboardAsync();
    Task<List<MyCourseDto>> GetMyCoursesAsync();
    Task<CoursePlayerDto> GetCoursePlayerAsync(int courseId);
    Task<SectionContentDto> GetSectionContentAsync(int sectionId);
    Task<List<QuizQuestionDto>> GetSectionQuizAsync(int sectionId);
    Task<QuickAccessDto> GetQuickAccessAsync(int courseId);
    Task<int> AddFeedbackAsync(FeedbackRequest request);
    Task UpdateFeedbackAsync(int id, FeedbackRequest request);
    Task DeleteFeedbackAsync(int id);
    Task SubmitAssignmentAsync(int assignmentId, SubmitAssignmentRequest request);
}

public class StudentService(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICatalogService catalog) : IStudentService
{
    private string Me => currentUser.RequiredUserId;

    public async Task<StudentDashboardDto> GetDashboardAsync()
    {
        var me = Me;
        var courses = await unitOfWork.Repository<Enrollment>().SelectAsync(
            e => new StudentCourseRowDto(e.Course!.CourseID, e.Course.CourseName, e.Course.StartDate, e.Course.EndDate),
            e => e.StudentId == me && e.Course != null);
        var courseIds = courses.Select(c => c.Id).Distinct().ToList();

        var schedule = await unitOfWork.Repository<ClassSchedule>().SelectAsync(
            s => new StudentScheduleDto(s.CourseId, s.Course.CourseName, s.StartTime),
            s => courseIds.Contains(s.CourseId), q => q.OrderBy(s => s.StartTime));

        var feedbacks = await unitOfWork.Repository<Feedback>().SelectAsync(
            f => new ReceivedFeedbackDto(f.FeedbackID, f.ProviderUser!.FullName, f.ProviderUser.Picture, f.FeedbackDate, f.Rating, f.Content),
            f => f.TargetStudentUserId == me);

        // Legacy PostService.GetRelatedPostsByKeyWordsAsync: posts sharing a keyword with the student's courses.
        var keywords = (await unitOfWork.Repository<KeyWord>().SelectAsync(
            k => k.Name.ToLower(), k => k.CourseId != null && courseIds.Contains(k.CourseId.Value))).Distinct().ToList();
        var posts = keywords.Count == 0
            ? []
            : await unitOfWork.Repository<Post>().SelectAsync(
                p => new RelatedPostDto(p.Id, p.Content, p.Attachment, p.CreatedAt, p.User.UserName, p.User.Picture, p.CommunityId),
                p => p.KeyWords.Any(k => keywords.Contains(k.Name.ToLower())),
                q => q.OrderByDescending(p => p.CreatedAt));

        var communityIds = posts.Where(p => p.CommunityId != null).Select(p => p.CommunityId!.Value).Distinct().ToList();
        var communities = await unitOfWork.Repository<Community>().SelectAsync(
            c => new SuggestedCommunityDto(c.Id, c.Name, c.Description), c => communityIds.Contains(c.Id));

        return new StudentDashboardDto(courses, schedule, feedbacks, await catalog.GetRecommendationsAsync(me), posts, communities);
    }

    /// <summary>Technical fix: legacy loaded every student's progress rows (ViewBag.prog).</summary>
    public Task<List<MyCourseDto>> GetMyCoursesAsync()
    {
        var me = Me;
        return unitOfWork.Repository<Enrollment>().SelectAsync(
            e => new MyCourseDto(e.Course!.CourseID, e.Course.CourseName, FilePaths.Cover(e.Course.ImgCover),
                e.Course.Instructor != null ? e.Course.Instructor.FullName : null, e.Course.Rate,
                e.Course.StudentCourseProgresses.Where(p => p.StudentId == me).Select(p => p.ProgressPercentage).FirstOrDefault()),
            e => e.StudentId == me && e.Course != null);
    }

    public async Task<CoursePlayerDto> GetCoursePlayerAsync(int courseId)
    {
        await EnsureEnrolledAsync(courseId);
        var me = Me;
        return (await unitOfWork.Repository<Course>().SelectOneAsync(c => c.CourseID == courseId, c => new CoursePlayerDto(
            c.CourseID, c.CourseName,
            c.CourseCurricula.OrderBy(s => s.CourseCurriculumID).Select(s => new SectionLinkDto(s.CourseCurriculumID, s.Title)).ToList(),
            c.Feedbacks.OrderByDescending(f => f.FeedbackDate).Select(f => new CourseFeedbackDto(f.FeedbackID, f.ProviderUser!.FullName,
                f.ProviderUser.Picture, f.FeedbackDate, f.Rating, f.Content, f.ProviderUserId == me)).ToList())))!;
    }

    public async Task<SectionContentDto> GetSectionContentAsync(int sectionId)
    {
        await EnsureEnrolledInSectionAsync(sectionId);
        var me = Me;
        return (await unitOfWork.Repository<CourseCurriculum>().SelectOneAsync(s => s.CourseCurriculumID == sectionId, s => new SectionContentDto(
            s.CourseCurriculumID, s.Title, s.Content,
            s.CourseVideos.Select(v => new PlayerVideoDto(v.CourseVideoID, v.VideoTitle, v.VideoURL)).ToList(),
            s.CourseResources.Select(r => new PlayerResourceDto(r.CourseResourceID, r.ResourceTitle, r.ResourceURL)).ToList(),
            // Technical fix: legacy marked an assignment "Submitted" when ANY student had submitted it.
            s.Assignments.Select(a => new PlayerAssignmentDto(a.AssignmentID, a.Title, a.Content, a.DeadLine,
                a.Submissions.Any(x => x.ApplicationUserID == me))).ToList(),
            s.Feedbacks.Select(f => new CourseFeedbackDto(f.FeedbackID, f.ProviderUser!.UserName, f.ProviderUser.Picture,
                f.FeedbackDate, f.Rating, f.Content, f.ProviderUserId == me)).ToList(),
            s.Exams.Any())))!;
    }

    /// <summary>Legacy GetQuestions(curriculumId): questions + choice texts of the section's quiz.</summary>
    public async Task<List<QuizQuestionDto>> GetSectionQuizAsync(int sectionId)
    {
        await EnsureEnrolledInSectionAsync(sectionId);
        var examId = await unitOfWork.Repository<Exam>().SelectOneAsync(e => e.CurriculumId == sectionId, e => (int?)e.ExamID)
                     ?? throw new NotFoundException("Exam not found");
        return await unitOfWork.Repository<Question>().SelectAsync(
            q => new QuizQuestionDto(q.QuestionText, q.Choices.Select(c => c.ChoiceText).ToList()), q => q.ExamID == examId);
    }

    public async Task<QuickAccessDto> GetQuickAccessAsync(int courseId)
    {
        await EnsureEnrolledAsync(courseId);
        var me = Me;
        return (await unitOfWork.Repository<Course>().SelectOneAsync(c => c.CourseID == courseId, c => new QuickAccessDto(
            c.CourseID, c.CourseName,
            c.CourseCurricula.Select(s => new QuickAccessSectionDto(s.Title, s.Content,
                s.CourseVideos.Select(v => new PlayerVideoDto(v.CourseVideoID, v.VideoTitle, v.VideoURL)).ToList(),
                s.CourseResources.Select(r => new PlayerResourceDto(r.CourseResourceID, r.ResourceTitle, r.ResourceURL)).ToList(),
                s.Assignments.Select(a => new PlayerAssignmentDto(a.AssignmentID, a.Title, a.Content, a.DeadLine,
                    a.Submissions.Any(x => x.ApplicationUserID == me))).ToList())).ToList())))!;
    }

    public async Task<int> AddFeedbackAsync(FeedbackRequest request)
    {
        await EnsureFeedbackTargetAsync(request);
        var feedback = new Feedback
        {
            CourseId = request.CourseId,
            CourseCurriculumId = request.CourseCurriculumId,
            Rating = request.Rating,
            Content = request.Content,
            ProviderUserId = Me, // technical fix: legacy trusted a hidden form field
            FeedbackDate = DateTime.Now,
        };
        await unitOfWork.Repository<Feedback>().AddAsync(feedback);
        await unitOfWork.SaveChangesAsync();
        await RecalculateCourseRateAsync(feedback.CourseId);
        return feedback.FeedbackID;
    }

    public async Task UpdateFeedbackAsync(int id, FeedbackRequest request)
    {
        var feedback = await GetOwnFeedbackAsync(id);
        feedback.Rating = request.Rating;
        feedback.Content = request.Content;
        await unitOfWork.SaveChangesAsync();
        await RecalculateCourseRateAsync(feedback.CourseId);
    }

    /// <summary>Technical fix: legacy deleted any feedback by id and then redirected to a URL taken from the query string.</summary>
    public async Task DeleteFeedbackAsync(int id)
    {
        var feedback = await GetOwnFeedbackAsync(id);
        unitOfWork.Repository<Feedback>().Delete(feedback);
        await unitOfWork.SaveChangesAsync();
        await RecalculateCourseRateAsync(feedback.CourseId);
    }

    /// <summary>
    /// Legacy StudentAssinmentController.UploadSoluation: one submission per student and assignment.
    /// (The legacy form posted to a controller that did not have this action, so it never worked.)
    /// </summary>
    public async Task SubmitAssignmentAsync(int assignmentId, SubmitAssignmentRequest request)
    {
        var sectionId = await unitOfWork.Repository<Assignment>().SelectOneAsync(a => a.AssignmentID == assignmentId, a => (int?)a.CourseCurriculumID)
                        ?? throw new NotFoundException(nameof(Assignment), assignmentId);
        await EnsureEnrolledInSectionAsync(sectionId);

        var me = Me;
        if (await unitOfWork.Repository<AssignmentSubmission>().AnyAsync(s => s.AssignmentID == assignmentId && s.ApplicationUserID == me))
            throw new BadRequestException("You have already submitted this assignment.");

        await unitOfWork.Repository<AssignmentSubmission>().AddAsync(new AssignmentSubmission
        {
            AssignmentID = assignmentId,
            ApplicationUserID = me,
            SubmissionDate = DateTime.Now,
            Content = request.Content,
        });
        await unitOfWork.SaveChangesAsync();
    }

    // ---------------------------------------------------------------- helpers

    /// <summary>Technical / security fix: legacy opened any course's paid content without checking the enrollment.</summary>
    private async Task EnsureEnrolledAsync(int courseId)
    {
        if (!await unitOfWork.Repository<Course>().AnyAsync(c => c.CourseID == courseId))
            throw new NotFoundException(nameof(Course), courseId);
        var me = Me;
        if (!await unitOfWork.Repository<Enrollment>().AnyAsync(e => e.CourseID == courseId && e.StudentId == me))
            throw new ForbiddenException("You are not enrolled in this course.");
    }

    private async Task EnsureEnrolledInSectionAsync(int sectionId)
    {
        var courseId = await unitOfWork.Repository<CourseCurriculum>().SelectOneAsync(s => s.CourseCurriculumID == sectionId, s => (int?)s.CourseID)
                       ?? throw new NotFoundException("Curriculum", sectionId);
        await EnsureEnrolledAsync(courseId);
    }

    private async Task EnsureFeedbackTargetAsync(FeedbackRequest request)
    {
        if (request.CourseId is { } courseId) await EnsureEnrolledAsync(courseId);
        if (request.CourseCurriculumId is { } sectionId) await EnsureEnrolledInSectionAsync(sectionId);
    }

    private async Task<Feedback> GetOwnFeedbackAsync(int id)
    {
        var feedback = await unitOfWork.Repository<Feedback>().GetOneAsync(f => f.FeedbackID == id, tracked: true)
                       ?? throw new NotFoundException(nameof(Feedback), id);
        if (feedback.ProviderUserId != Me) throw new ForbiddenException();
        return feedback;
    }

    /// <summary>Legacy CourseService.EditRatingCourseBasedOnReviews: Course.Rate = (int) average rating.</summary>
    private async Task RecalculateCourseRateAsync(int? courseId)
    {
        if (courseId is null) return;
        var ratings = await unitOfWork.Repository<Feedback>().SelectAsync(f => f.Rating, f => f.CourseId == courseId && f.Rating != null);
        var course = await unitOfWork.Repository<Course>().GetOneAsync(c => c.CourseID == courseId, tracked: true);
        if (course is null) return;

        course.Rate = ratings.Count == 0 ? null : (int)ratings.Average(r => r!.Value);
        await unitOfWork.SaveChangesAsync();
    }
}

using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Domain.Constants;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Catalog;

/// <summary>Public storefront pages (legacy Customer/HomeController + CourseService read methods).</summary>
public interface ICatalogService
{
    Task<HomeDto> GetHomeAsync(PaginationParams pagination);
    Task<DepartmentCoursesDto> GetCoursesByDepartmentAsync(int? departmentId, PaginationParams pagination);
    Task<PagedResult<CourseCardDto>> SearchCoursesAsync(CourseFilter filter, PaginationParams pagination);
    Task<CourseDetailsDto> GetCourseDetailsAsync(int id, int? starFilter);
    Task<List<DepartmentCardDto>> GetCategoriesAsync();
    Task<CategoryDto> GetCategoryAsync(int id);
    Task<List<CourseCardDto>> GetTopicCoursesAsync(string word);
    Task<PublicTrackDto> GetTrackAsync(int id);
    Task<List<NavCategoryDto>> GetNavCategoriesAsync();
}

public class CatalogService(IUnitOfWork unitOfWork, ICacheService cache, ICurrentUser currentUser) : ICatalogService
{
    private IGenericRepository<Course> Courses => unitOfWork.Repository<Course>();

    /// <summary>Student id when the current user is a student (recommendations / enrolled flags).</summary>
    private string? StudentId => currentUser.IsInRole(Roles.Student) ? currentUser.UserId : null;

    public async Task<HomeDto> GetHomeAsync(PaginationParams pagination)
    {
        var studentId = StudentId;
        var userId = currentUser.UserId;

        return new HomeDto(
            await CachedAsync(CacheKeys.TopCourses, GetTopCoursesAsync),
            await CachedAsync(CacheKeys.TopDepartments, GetTopDepartmentsAsync),
            await CachedAsync(CacheKeys.TopTracks, GetTopTracksAsync),
            await Courses.GetPagedAsync(CourseCardDto.Projection, pagination),
            studentId is null ? [] : await GetRecommendationsAsync(studentId),
            userId is null ? [] : await unitOfWork.Repository<Enrollment>().SelectAsync(
                e => e.CourseID!.Value, e => e.StudentId == userId && e.CourseID != null),
            await unitOfWork.Repository<Feedback>().SelectAsync(
                f => new TestimonialDto(f.FeedbackID, f.ProviderUser!.FullName, f.ProviderUser.Picture,
                    f.CourseId!.Value, f.Course!.CourseName, f.Content, f.FeedbackDate),
                f => f.CourseId != null),
            await unitOfWork.Repository<Event>().SelectAsync(e => new PublicEventDto(e.EventID, e.Date, e.Name, e.Description,
                e.CreatedBy != null ? e.CreatedBy.FullName : null, e.Department != null ? e.Department.DepartmentName : null)));
    }

    public async Task<DepartmentCoursesDto> GetCoursesByDepartmentAsync(int? departmentId, PaginationParams pagination)
    {
        if (departmentId is null or 0)
            return new DepartmentCoursesDto(null, null, await Courses.GetPagedAsync(CourseCardDto.Projection, pagination));

        var department = await unitOfWork.Repository<Department>().GetOneAsync(d => d.DepartmentID == departmentId);
        return new DepartmentCoursesDto(department?.DepartmentName, department?.DepartmentDescription,
            await Courses.GetPagedAsync(CourseCardDto.Projection, pagination, c => c.DepartmentId == departmentId));
    }

    /// <summary>
    /// Legacy CourseRepository.GetFilteredCourses. Technical fix: the legacy page computed the
    /// filtered query but displayed the unfiltered list; the "Rating &amp; Up" filter was not applied.
    /// </summary>
    public Task<PagedResult<CourseCardDto>> SearchCoursesAsync(CourseFilter filter, PaginationParams pagination)
    {
        var search = filter.Search?.Trim().ToLower();

        return Courses.GetPagedAsync(CourseCardDto.Projection, pagination,
            c => (string.IsNullOrEmpty(search) || c.CourseName.ToLower().Contains(search))
                 && (string.IsNullOrEmpty(filter.InstructorId) || c.InstructorId == filter.InstructorId)
                 && (filter.CategoryId == null || filter.CategoryId <= 0 || c.DepartmentId == filter.CategoryId)
                 && (filter.Price == null || filter.Price <= 0 || c.Price <= filter.Price)
                 && (filter.Rate == null || filter.Rate <= 0 || c.Rate >= filter.Rate),
            q =>
            {
                var desc = filter.SortOrder?.ToLower() == "desc";
                return filter.SortBy?.ToLower() switch
                {
                    "price" => desc ? q.OrderByDescending(c => c.Price) : q.OrderBy(c => c.Price),
                    "rate" => desc ? q.OrderByDescending(c => c.Rate) : q.OrderBy(c => c.Rate),
                    "popularity" => desc ? q.OrderByDescending(c => c.Enrollments.Count) : q.OrderBy(c => c.Enrollments.Count),
                    _ => q.OrderByDescending(c => c.CourseID),
                };
            });
    }

    public async Task<CourseDetailsDto> GetCourseDetailsAsync(int id, int? starFilter)
    {
        var course = await Courses.SelectOneAsync(c => c.CourseID == id, c => new
        {
            c.CourseID, c.CourseName, c.Description, c.Rate, c.Price, c.InstructorId, c.DepartmentId, c.TrackID,
            c.DemoVideoUrl,
            // Legacy: the demo is the first video of the first curriculum section.
            FirstVideo = c.CourseCurricula.SelectMany(cc => cc.CourseVideos)
                .Select(v => new { v.VideoURL, v.VideoTitle }).FirstOrDefault(),
            NumOfFeedbacks = c.Feedbacks.Count,
            NumOfVideos = c.CourseVideos.Count,
            NumOfEnrollments = c.Enrollments.Count,
            InstructorName = c.Instructor != null ? c.Instructor.UserName : null,
            InstructorBio = c.Instructor != null ? c.Instructor.JopDescription : null,
            InstructorImage = c.Instructor != null ? c.Instructor.Picture : null,
            Objectives = c.LearningObjectives!.Select(o => o.Objective).ToList(),
            Topics = c.TopicsCovered!.Select(t => t.Topic).ToList(),
        }) ?? throw new NotFoundException(nameof(Course), id); // legacy threw a NullReferenceException

        var (summary, reviews) = await GetReviewsAsync(id, starFilter);

        return new CourseDetailsDto(
            course.CourseID, course.CourseName, course.Description,
            course.FirstVideo?.VideoURL ?? course.DemoVideoUrl, course.FirstVideo?.VideoTitle,
            course.Rate, course.Price, course.NumOfFeedbacks, course.NumOfVideos, course.NumOfEnrollments,
            course.InstructorId, course.InstructorName, course.InstructorBio, course.InstructorImage,
            course.DepartmentId, course.TrackID, course.Objectives, course.Topics,
            await GetRelatedCoursesAsync(id, course.TrackID, course.DepartmentId),
            await Courses.SelectAsync(CourseCardDto.Projection, c => c.InstructorId == course.InstructorId && c.CourseID != id),
            summary,
            reviews);
    }

    public Task<List<DepartmentCardDto>> GetCategoriesAsync()
        => unitOfWork.Repository<Department>().SelectAsync(d =>
            new DepartmentCardDto(d.DepartmentID, d.DepartmentName, d.DepartmentDescription, FilePaths.Cover(d.Cover)));

    public async Task<CategoryDto> GetCategoryAsync(int id)
    {
        var department = await unitOfWork.Repository<Department>().GetOneAsync(d => d.DepartmentID == id)
                         ?? throw new NotFoundException(nameof(Department), id);

        var keyWords = await unitOfWork.Repository<KeyWord>().SelectAsync(
            k => k.Name, k => k.Course != null && k.Course.DepartmentId == id);

        return new CategoryDto(department.DepartmentID, department.DepartmentName, department.DepartmentDescription,
            await Courses.SelectAsync(CourseCardDto.Projection, c => c.DepartmentId == id),
            keyWords.Distinct().ToList());
    }

    /// <summary>Courses whose keyword or name contains the word (legacy Home/TopicCourse).</summary>
    public Task<List<CourseCardDto>> GetTopicCoursesAsync(string word)
    {
        var term = word.Trim().ToLower();
        return Courses.SelectAsync(CourseCardDto.Projection,
            c => c.Keywords.Any(k => k.Name.ToLower().Contains(term)) || c.CourseName.ToLower().Contains(term));
    }

    public async Task<PublicTrackDto> GetTrackAsync(int id)
    {
        var track = await unitOfWork.Repository<Track>().SelectOneAsync(t => t.Id == id, t => new
        {
            t.Id, t.Name, t.Description, t.Cover,
            NumOfCourses = t.Courses.Count,
            NumOfEnrollments = t.Courses.SelectMany(c => c.Enrollments).Count(),
        }) ?? throw new NotFoundException(nameof(Track), id);

        var studentId = StudentId;
        var notOwned = studentId is null
            ? []
            : await Courses.SelectAsync(CourseCardDto.Projection,
                c => c.TrackID == id && !c.Enrollments.Any(e => e.StudentId == studentId));

        return new PublicTrackDto(track.Id, track.Name, track.Description, FilePaths.Cover(track.Cover),
            track.NumOfCourses, track.NumOfEnrollments,
            // Technical fix: legacy never filled this number, so the track progress bar was always 0%.
            studentId is null ? 0 : track.NumOfCourses - notOwned.Count,
            await Courses.SelectAsync(CourseCardDto.Projection, c => c.TrackID == id),
            notOwned);
    }

    public Task<List<NavCategoryDto>> GetNavCategoriesAsync()
        => unitOfWork.Repository<Department>().SelectAsync(d => new NavCategoryDto(d.DepartmentID, d.DepartmentName,
            d.Courses.Select(c => new LookupItem<int>(c.CourseID, c.CourseName)).ToList()));

    // ---------------------------------------------------------------- helpers

    private async Task<List<T>> CachedAsync<T>(string key, Func<Task<List<T>>> load)
    {
        var cached = await cache.GetAsync<List<T>>(key);
        if (cached is not null) return cached;

        var value = await load();
        await cache.SetAsync(key, value, CacheKeys.HomeTtl);
        return value;
    }

    /// <summary>Legacy CourseService.GetCoursesWIthMaxEnrollments: the 5 courses with most enrollments.</summary>
    private Task<List<CourseCardDto>> GetTopCoursesAsync()
        => Courses.SelectAsync(CourseCardDto.Projection, c => c.Enrollments.Any(),
            q => q.OrderByDescending(c => c.Enrollments.Count), take: 5);

    /// <summary>Legacy DepartmentRepository.GetTopDepartments: departments of the 3 most enrolled courses.</summary>
    private async Task<List<DepartmentCardDto>> GetTopDepartmentsAsync()
    {
        var departmentIds = await Courses.SelectAsync(c => c.DepartmentId,
            c => c.DepartmentId != null, q => q.OrderByDescending(c => c.Enrollments.Count), take: 3);
        var ids = departmentIds.Distinct().ToList();

        return await unitOfWork.Repository<Department>().SelectAsync(
            d => new DepartmentCardDto(d.DepartmentID, d.DepartmentName, d.DepartmentDescription, FilePaths.Cover(d.Cover)),
            d => ids.Contains(d.DepartmentID));
    }

    /// <summary>Legacy TrackRepository.GetTopTracksAsync: the 3 tracks with most enrollments.</summary>
    private Task<List<TrackCardDto>> GetTopTracksAsync()
        => unitOfWork.Repository<Track>().SelectAsync(
            t => new TrackCardDto(t.Id, t.Name, t.Description, FilePaths.Cover(t.Cover), t.Courses.Count,
                t.Courses.SelectMany(c => c.Enrollments).Count()),
            orderBy: q => q.OrderByDescending(t => t.Courses.SelectMany(c => c.Enrollments).Count()),
            take: 3);

    /// <summary>
    /// Legacy CourseService.GetRecommendationCoursesforSpacifcStudent: courses in the student's tracks or
    /// departments that they don't have yet (first page, 3 courses).
    /// Technical fix: courses without a track/department no longer match every other course without one.
    /// </summary>
    private async Task<List<CourseCardDto>> GetRecommendationsAsync(string studentId)
    {
        var owned = await unitOfWork.Repository<Enrollment>().SelectAsync(
            e => new { e.CourseID, e.Course!.TrackID, e.Course.DepartmentId },
            e => e.StudentId == studentId && e.Course != null);

        var courseIds = owned.Select(o => o.CourseID!.Value).Distinct().ToList();
        var trackIds = owned.Where(o => o.TrackID != null).Select(o => o.TrackID!.Value).Distinct().ToList();
        var departmentIds = owned.Where(o => o.DepartmentId != null).Select(o => o.DepartmentId!.Value).Distinct().ToList();

        var page = await Courses.GetPagedAsync(CourseCardDto.Projection, new PaginationParams(),
            c => ((c.TrackID != null && trackIds.Contains(c.TrackID.Value)) ||
                  (c.DepartmentId != null && departmentIds.Contains(c.DepartmentId.Value)))
                 && c.InstructorId != studentId && !courseIds.Contains(c.CourseID));
        return page.Items.ToList();
    }

    /// <summary>Legacy CourseService.GetRelatedCourses: same track and department, plus the student's recommendations.</summary>
    private async Task<List<CourseCardDto>> GetRelatedCoursesAsync(int courseId, int? trackId, int? departmentId)
    {
        var recommendedIds = StudentId is { } studentId
            ? (await GetRecommendationsAsync(studentId)).Select(c => c.Id).ToList()
            : [];

        return await Courses.SelectAsync(CourseCardDto.Projection,
            c => c.CourseID != courseId &&
                 ((c.TrackID == trackId && c.DepartmentId == departmentId) || recommendedIds.Contains(c.CourseID)));
    }

    /// <summary>Legacy CourseService.GetCourseFeedbackAndReviews: star histogram + the 3 newest reviews.</summary>
    private async Task<(ReviewSummaryDto, List<ReviewDto>)> GetReviewsAsync(int courseId, int? star)
    {
        var feedbacks = unitOfWork.Repository<Feedback>();
        var ratings = await feedbacks.SelectAsync(f => f.Rating, f => f.CourseId == courseId);
        var rated = ratings.Where(r => r != null).Select(r => r!.Value).ToList();

        var summary = new ReviewSummaryDto(
            ratings.Count,
            rated.Count == 0 ? 0 : (int)Math.Round(rated.Average()),
            Enumerable.Range(1, 5).ToDictionary(s => s, s => rated.Count(r => r == s)));

        var reviews = await feedbacks.SelectAsync(
            f => new ReviewDto(f.FeedbackID, f.Content, f.FeedbackDate, f.Rating,
                f.ProviderUser!.FullName ?? "Unknown", f.ProviderUser.Picture),
            f => f.CourseId == courseId && (star == null || f.Rating == star),
            q => q.OrderByDescending(f => f.FeedbackDate),
            take: 3);

        return (summary, reviews);
    }
}

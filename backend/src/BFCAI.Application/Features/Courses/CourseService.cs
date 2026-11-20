using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Features.Courses;

/// <summary>
/// Course management (legacy Admin/CourseController; the instructor area reuses it in Phase 5).
/// </summary>
public interface ICourseManagementService
{
    Task<List<AdminCourseRowDto>> GetAllAsync();
    Task<AdminCourseDetailsDto> GetDetailsAsync(int id);
    Task<CourseFormDto> GetFormAsync(int id);
    Task<CourseFormDto> CreateAsync(CourseRequest request);
    Task<CourseFormDto> UpdateAsync(int id, CourseRequest request);
    Task DeleteAsync(int id);
}

public class CourseManagementService(IUnitOfWork unitOfWork, IFileStorage files, ICacheService cache)
    : ICourseManagementService
{
    private IGenericRepository<Course> Courses => unitOfWork.Repository<Course>();

    public Task<List<AdminCourseRowDto>> GetAllAsync() => Courses.SelectAsync(AdminCourseRowDto.Projection);

    public async Task<AdminCourseDetailsDto> GetDetailsAsync(int id)
        => await Courses.SelectOneAsync(c => c.CourseID == id, c => new AdminCourseDetailsDto(
               c.CourseID, c.CourseName, c.Description, c.Credits,
               c.Instructor != null ? c.Instructor.UserName : null,
               c.Department != null ? c.Department.DepartmentName : null,
               c.Enrollments.Count))
           ?? throw new NotFoundException(nameof(Course), id);

    public async Task<CourseFormDto> GetFormAsync(int id)
        => await Courses.SelectOneAsync(c => c.CourseID == id, c => new CourseFormDto(
               c.CourseID, c.CourseName, c.Description, c.Credits, c.Price, c.Rate, c.DemoVideoUrl,
               c.NumOfEnrollments, c.StartDate, c.EndDate, c.InstructorId, c.DepartmentId, c.TrackID,
               FilePaths.Cover(c.ImgCover),
               c.Keywords.Select(k => k.Name).ToList(),
               c.LearningObjectives!.Select(o => o.Objective).ToList(),
               c.TopicsCovered!.Select(t => t.Topic).ToList()))
           ?? throw new NotFoundException(nameof(Course), id);

    public async Task<CourseFormDto> CreateAsync(CourseRequest request)
    {
        if (request.ImgCover is null) Validation.Fail(nameof(request.ImgCover), "Please upload an image.");

        var course = new Course();
        Apply(course, request);
        course.ImgCover = await SaveCoverAsync(request);
        ReplaceLists(course, request);

        await Courses.AddAsync(course);
        await unitOfWork.SaveChangesAsync(); // one save = one transaction (legacy used ~6 commits)
        await cache.RemoveAsync(CacheKeys.Catalog);
        return await GetFormAsync(course.CourseID);
    }

    public async Task<CourseFormDto> UpdateAsync(int id, CourseRequest request)
    {
        var course = await Courses.GetOneAsync(c => c.CourseID == id, tracked: true,
                         c => c.Keywords, c => c.LearningObjectives, c => c.TopicsCovered)
                     ?? throw new NotFoundException(nameof(Course), id);

        Apply(course, request);
        if (request.ImgCover is not null)
        {
            // Technical fix: legacy cleared the cover when editing without a new image.
            var oldCover = course.ImgCover;
            course.ImgCover = await SaveCoverAsync(request);
            await files.DeleteAsync($"{FilePaths.CoversFolder}/{oldCover}");
        }

        // Legacy: delete the old topics / objectives / keywords and insert the new ones.
        unitOfWork.Repository<KeyWord>().DeleteRange(course.Keywords);
        unitOfWork.Repository<LearningObjective>().DeleteRange(course.LearningObjectives!);
        unitOfWork.Repository<TopicCovered>().DeleteRange(course.TopicsCovered!);
        ReplaceLists(course, request);

        await unitOfWork.SaveChangesAsync();
        await cache.RemoveAsync(CacheKeys.Catalog);
        return await GetFormAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var course = await Courses.GetOneAsync(c => c.CourseID == id, tracked: true)
                     ?? throw new NotFoundException(nameof(Course), id);

        Courses.Delete(course);
        await unitOfWork.SaveChangesAsync();
        await files.DeleteAsync($"{FilePaths.CoversFolder}/{course.ImgCover}");
        await cache.RemoveAsync(CacheKeys.Catalog);
    }

    private static void Apply(Course course, CourseRequest request)
    {
        course.CourseName = request.CourseName;
        course.Description = request.Description;
        course.Credits = request.Credits;
        course.Price = request.Price;
        course.Rate = request.Rate;
        course.DemoVideoUrl = request.DemoVideoUrl;
        course.NumOfEnrollments = request.NumOfEnrollments;
        course.StartDate = request.StartDate;
        course.EndDate = request.EndDate;
        course.InstructorId = request.InstructorId ?? string.Empty;
        course.DepartmentId = request.DepartmentId;
        course.TrackID = request.TrackId;
    }

    /// <summary>Empty entries are ignored (the form can add empty rows).</summary>
    private static void ReplaceLists(Course course, CourseRequest request)
    {
        course.Keywords = Clean(request.KeyWords).Select(n => new KeyWord { Name = n }).ToList();
        course.LearningObjectives = Clean(request.LearningObjectives).Select(o => new LearningObjective { Objective = o }).ToList();
        course.TopicsCovered = Clean(request.TopicsCovered).Select(t => new TopicCovered { Topic = t }).ToList();
    }

    private static IEnumerable<string> Clean(List<string>? values)
        => (values ?? []).Where(v => !string.IsNullOrWhiteSpace(v)).Select(v => v.Trim());

    private Task<string> SaveCoverAsync(CourseRequest request)
        => files.SaveAsync(request.ImgCover!, FilePaths.CoversFolder, FilePaths.NewCoverName(request.ImgCover!.FileName));
}

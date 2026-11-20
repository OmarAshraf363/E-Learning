using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Domain.Entities;
using FluentValidation;

namespace BFCAI.Application.Features.Instructor;

public record CurriculumVideoDto(int Id, string Title, string Url);
public record CurriculumReferenceDto(int Id, string Title, string Url);
public record CurriculumAssignmentDto(int Id, string Title, string Description, DateTime DeadLine, string Content);
public record CurriculumExamDto(int Id, string Title, DateTime ExamDate);

public record CurriculumSectionDto(
    int Id, string Title, string Content,
    List<CurriculumVideoDto> Videos,
    List<CurriculumReferenceDto> References,
    List<CurriculumAssignmentDto> Assignments,
    List<CurriculumExamDto> Exams);

/// <summary>Legacy _CourseCurriculumPartial (course sections with videos, references, assignments, quizzes).</summary>
public record CourseCurriculumDto(int CourseId, string CourseName, List<CurriculumSectionDto> Sections);

public record CurriculumSectionRequest(string Title, string Content);
public record ReferenceRequest(string ResourceTitle, string ResourceURL);
public record VideoUploadRequest(string Title, FileUpload? Video);
public record QuizRequest(string Title, DateTime? ExamDate);

public class CurriculumSectionRequestValidator : AbstractValidator<CurriculumSectionRequest>
{
    public CurriculumSectionRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
    }
}

public class ReferenceRequestValidator : AbstractValidator<ReferenceRequest>
{
    public ReferenceRequestValidator()
    {
        RuleFor(x => x.ResourceTitle).NotEmpty();
        RuleFor(x => x.ResourceURL).NotEmpty();
    }
}

public class VideoUploadRequestValidator : AbstractValidator<VideoUploadRequest>
{
    public VideoUploadRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Video).NotNull().WithMessage("Please choose a video file.");
        RuleFor(x => x.Video!).Must(f => FileRules.HasAllowedExtension(f, FileRules.VideoExtensions))
            .When(x => x.Video is not null).WithMessage("Only video files are allowed.");
    }
}

public class QuizRequestValidator : AbstractValidator<QuizRequest>
{
    public QuizRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ExamDate).NotNull();
    }
}

/// <summary>
/// Course sections and their content (legacy InstructorController curriculum / video / reference / quiz actions).
/// The legacy management view could not be opened (its "View" button called a missing JS function and
/// GetCourseCurriculum passed the wrong model); it is implemented here as intended.
/// </summary>
public interface ICurriculumService
{
    Task<CourseCurriculumDto> GetAsync(int courseId);
    Task<CurriculumSectionDto> AddSectionAsync(int courseId, CurriculumSectionRequest request);
    Task UpdateSectionAsync(int sectionId, CurriculumSectionRequest request);
    Task<CurriculumVideoDto> UploadVideoAsync(int sectionId, VideoUploadRequest request);
    Task DeleteVideoAsync(int videoId);
    Task<CurriculumReferenceDto> AddReferenceAsync(int sectionId, ReferenceRequest request);
    Task UpdateReferenceAsync(int referenceId, ReferenceRequest request);
    Task DeleteReferenceAsync(int referenceId);
    Task<CurriculumExamDto> AddQuizAsync(int sectionId, QuizRequest request);
    Task UpdateQuizAsync(int examId, QuizRequest request);
    Task DeleteQuizAsync(int examId);
}

public class CurriculumService(IUnitOfWork unitOfWork, InstructorAccess access, IFileStorage files) : ICurriculumService
{
    private const string VideosFolder = "videos";

    public async Task<CourseCurriculumDto> GetAsync(int courseId)
    {
        await access.EnsureOwnsCourseAsync(courseId);
        return (await unitOfWork.Repository<Course>().SelectOneAsync(c => c.CourseID == courseId, c => new CourseCurriculumDto(
            c.CourseID, c.CourseName,
            c.CourseCurricula.OrderBy(s => s.CourseCurriculumID).Select(s => new CurriculumSectionDto(
                s.CourseCurriculumID, s.Title, s.Content,
                s.CourseVideos.Select(v => new CurriculumVideoDto(v.CourseVideoID, v.VideoTitle, v.VideoURL)).ToList(),
                s.CourseResources.Select(r => new CurriculumReferenceDto(r.CourseResourceID, r.ResourceTitle, r.ResourceURL)).ToList(),
                s.Assignments.Select(a => new CurriculumAssignmentDto(a.AssignmentID, a.Title, a.Description, a.DeadLine, a.Content)).ToList(),
                s.Exams.Select(e => new CurriculumExamDto(e.ExamID, e.Title, e.ExamDate)).ToList())).ToList())))!;
    }

    public async Task<CurriculumSectionDto> AddSectionAsync(int courseId, CurriculumSectionRequest request)
    {
        await access.EnsureOwnsCourseAsync(courseId);
        var section = new CourseCurriculum { CourseID = courseId, Title = request.Title, Content = request.Content };
        await unitOfWork.Repository<CourseCurriculum>().AddAsync(section);
        await unitOfWork.SaveChangesAsync();
        return new CurriculumSectionDto(section.CourseCurriculumID, section.Title, section.Content, [], [], [], []);
    }

    public async Task UpdateSectionAsync(int sectionId, CurriculumSectionRequest request)
    {
        await access.EnsureOwnsCurriculumAsync(sectionId);
        var section = (await unitOfWork.Repository<CourseCurriculum>().GetOneAsync(s => s.CourseCurriculumID == sectionId, tracked: true))!;
        section.Title = request.Title;
        section.Content = request.Content;
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>Legacy VedioService: wwwroot/videos/{guid}{ext}, stored as "videos/{file}".</summary>
    public async Task<CurriculumVideoDto> UploadVideoAsync(int sectionId, VideoUploadRequest request)
    {
        var courseId = await access.EnsureOwnsCurriculumAsync(sectionId);
        var fileName = await files.SaveAsync(request.Video!, VideosFolder, $"{Guid.NewGuid()}{request.Video!.Extension}");

        var video = new CourseVideo
        {
            VideoTitle = request.Title,
            VideoURL = $"{VideosFolder}/{fileName}",
            CourseCurriculumID = sectionId,
            CourseID = courseId,
        };
        await unitOfWork.Repository<CourseVideo>().AddAsync(video);
        await unitOfWork.SaveChangesAsync();
        return new CurriculumVideoDto(video.CourseVideoID, video.VideoTitle, video.VideoURL);
    }

    public async Task DeleteVideoAsync(int videoId)
    {
        var video = await unitOfWork.Repository<CourseVideo>().GetOneAsync(v => v.CourseVideoID == videoId, tracked: true)
                    ?? throw new NotFoundException(nameof(CourseVideo), videoId);
        await access.EnsureOwnsCourseAsync(video.CourseID);

        unitOfWork.Repository<CourseVideo>().Delete(video);
        await unitOfWork.SaveChangesAsync();
        // Technical fix: the legacy app left the file on disk.
        if (!video.VideoURL.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            await files.DeleteAsync(video.VideoURL);
    }

    public async Task<CurriculumReferenceDto> AddReferenceAsync(int sectionId, ReferenceRequest request)
    {
        await access.EnsureOwnsCurriculumAsync(sectionId);
        var reference = new CourseResource { CourseCurriculumID = sectionId, ResourceTitle = request.ResourceTitle, ResourceURL = request.ResourceURL };
        await unitOfWork.Repository<CourseResource>().AddAsync(reference);
        await unitOfWork.SaveChangesAsync(); // technical fix: the legacy AJAX save returned before Commit()
        return new CurriculumReferenceDto(reference.CourseResourceID, reference.ResourceTitle, reference.ResourceURL);
    }

    public async Task UpdateReferenceAsync(int referenceId, ReferenceRequest request)
    {
        var reference = await GetReferenceAsync(referenceId);
        reference.ResourceTitle = request.ResourceTitle;
        reference.ResourceURL = request.ResourceURL;
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteReferenceAsync(int referenceId)
    {
        var reference = await GetReferenceAsync(referenceId);
        unitOfWork.Repository<CourseResource>().Delete(reference);
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>Legacy UpsertQuiz: an exam attached to a section (course and instructor set by the server).</summary>
    public async Task<CurriculumExamDto> AddQuizAsync(int sectionId, QuizRequest request)
    {
        var courseId = await access.EnsureOwnsCurriculumAsync(sectionId);
        var exam = new Exam
        {
            Title = request.Title,
            ExamDate = request.ExamDate!.Value,
            CurriculumId = sectionId,
            CourseID = courseId,
            InstructorId = access.InstructorId,
        };
        await unitOfWork.Repository<Exam>().AddAsync(exam);
        await unitOfWork.SaveChangesAsync();
        return new CurriculumExamDto(exam.ExamID, exam.Title, exam.ExamDate);
    }

    public async Task UpdateQuizAsync(int examId, QuizRequest request)
    {
        await access.EnsureOwnsExamAsync(examId);
        var exam = (await unitOfWork.Repository<Exam>().GetOneAsync(e => e.ExamID == examId, tracked: true))!;
        exam.Title = request.Title;
        exam.ExamDate = request.ExamDate!.Value;
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteQuizAsync(int examId)
    {
        await access.EnsureOwnsExamAsync(examId);
        var exam = (await unitOfWork.Repository<Exam>().GetOneAsync(e => e.ExamID == examId, tracked: true))!;
        unitOfWork.Repository<Exam>().Delete(exam);
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<CourseResource> GetReferenceAsync(int referenceId)
    {
        var reference = await unitOfWork.Repository<CourseResource>().GetOneAsync(r => r.CourseResourceID == referenceId, tracked: true)
                        ?? throw new NotFoundException(nameof(CourseResource), referenceId);
        if (reference.CourseCurriculumID is { } sectionId) await access.EnsureOwnsCurriculumAsync(sectionId);
        else throw new ForbiddenException();
        return reference;
    }
}

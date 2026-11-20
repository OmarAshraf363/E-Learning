using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Domain.Entities;
using FluentValidation;

namespace BFCAI.Application.Features.Instructor;

public record AssignmentDto(int Id, int CurriculumId, string Title, string Description, DateTime DeadLine, string Content);
public record SectionAssignmentsDto(int Id, string Title, List<AssignmentDto> Assignments);
public record SubmissionDto(int Id, string? StudentName, DateTime SubmissionDate, string Content, int? Grade, string Feedback);

/// <summary>File (Content) is required on create, optional on edit.</summary>
public record AssignmentRequest(string Title, string? Description, DateTime? DeadLine, FileUpload? Content);

public class AssignmentRequestValidator : AbstractValidator<AssignmentRequest>
{
    public AssignmentRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.DeadLine).NotNull();
        RuleFor(x => x.Content!)
            .Must(f => FileRules.HasAllowedExtension(f, FileRules.AssignmentExtensions) && f.Length <= FileRules.AssignmentMaxBytes)
            .When(x => x.Content is not null)
            .WithMessage("Invalid file format or size. Please upload a valid file.");
    }
}

/// <summary>Legacy Instructor/AssinmentController.</summary>
public interface IAssignmentService
{
    Task<List<SectionAssignmentsDto>> GetByCourseAsync(int courseId);
    Task<AssignmentDto> GetAsync(int id);
    Task<AssignmentDto> CreateAsync(int curriculumId, AssignmentRequest request);
    Task<AssignmentDto> UpdateAsync(int id, AssignmentRequest request);
    Task DeleteAsync(int id);
    Task<List<SubmissionDto>> GetSubmissionsAsync(int id);
}

public class AssignmentService(IUnitOfWork unitOfWork, InstructorAccess access, IFileStorage files) : IAssignmentService
{
    private const string Folder = "assignments";
    private IGenericRepository<Assignment> Assignments => unitOfWork.Repository<Assignment>();

    public async Task<List<SectionAssignmentsDto>> GetByCourseAsync(int courseId)
    {
        await access.EnsureOwnsCourseAsync(courseId);
        return await unitOfWork.Repository<CourseCurriculum>().SelectAsync(
            s => new SectionAssignmentsDto(s.CourseCurriculumID, s.Title,
                s.Assignments.Select(a => new AssignmentDto(a.AssignmentID, a.CourseCurriculumID, a.Title, a.Description, a.DeadLine, a.Content)).ToList()),
            s => s.CourseID == courseId);
    }

    public async Task<AssignmentDto> GetAsync(int id)
    {
        var assignment = await GetOwnedAsync(id);
        return ToDto(assignment);
    }

    public async Task<AssignmentDto> CreateAsync(int curriculumId, AssignmentRequest request)
    {
        await access.EnsureOwnsCurriculumAsync(curriculumId);
        if (request.Content is null) Validation.Fail(nameof(request.Content), "Please upload the assignment file.");

        var assignment = new Assignment
        {
            CourseCurriculumID = curriculumId,
            Title = request.Title,
            Description = request.Description ?? string.Empty,
            DeadLine = request.DeadLine!.Value,
            Content = await SaveFileAsync(request.Content!),
        };
        await Assignments.AddAsync(assignment);
        await unitOfWork.SaveChangesAsync();
        return ToDto(assignment);
    }

    /// <summary>Technical fix: legacy required re-uploading the file on every edit.</summary>
    public async Task<AssignmentDto> UpdateAsync(int id, AssignmentRequest request)
    {
        var assignment = await GetOwnedAsync(id, tracked: true);
        assignment.Title = request.Title;
        assignment.Description = request.Description ?? string.Empty;
        assignment.DeadLine = request.DeadLine!.Value;

        if (request.Content is not null)
        {
            var oldFile = assignment.Content;
            assignment.Content = await SaveFileAsync(request.Content);
            await files.DeleteAsync(oldFile);
        }

        await unitOfWork.SaveChangesAsync();
        return ToDto(assignment);
    }

    /// <summary>Technical fix: the legacy path combination never found the file, so it was never deleted.</summary>
    public async Task DeleteAsync(int id)
    {
        var assignment = await GetOwnedAsync(id, tracked: true);
        Assignments.Delete(assignment);
        await unitOfWork.SaveChangesAsync();
        await files.DeleteAsync(assignment.Content);
    }

    public async Task<List<SubmissionDto>> GetSubmissionsAsync(int id)
    {
        await GetOwnedAsync(id);
        return await unitOfWork.Repository<AssignmentSubmission>().SelectAsync(
            s => new SubmissionDto(s.AssignmentSubmissionID, s.ApplicationUser != null ? s.ApplicationUser.FullName : null,
                s.SubmissionDate, s.Content, s.Grade, s.Feedback),
            s => s.AssignmentID == id);
    }

    /// <summary>Legacy AssinmentService: wwwroot/assignments/{guid}_{name}, stored as "/assignments/...".</summary>
    private async Task<string> SaveFileAsync(FileUpload file)
    {
        var name = await files.SaveAsync(file, Folder, $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}");
        return $"/{Folder}/{name}";
    }

    private async Task<Assignment> GetOwnedAsync(int id, bool tracked = false)
    {
        var assignment = await Assignments.GetOneAsync(a => a.AssignmentID == id, tracked)
                         ?? throw new NotFoundException(nameof(Assignment), id);
        await access.EnsureOwnsCurriculumAsync(assignment.CourseCurriculumID);
        return assignment;
    }

    private static AssignmentDto ToDto(Assignment a)
        => new(a.AssignmentID, a.CourseCurriculumID, a.Title, a.Description, a.DeadLine, a.Content);
}

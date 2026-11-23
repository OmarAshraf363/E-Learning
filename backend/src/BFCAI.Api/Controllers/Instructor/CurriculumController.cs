using BFCAI.Api.Infrastructure;
using BFCAI.Application.Features.Instructor;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Instructor;

/// <summary>Course sections and their videos / references / quizzes (legacy InstructorController).</summary>
[Route("api/instructor")]
public class CurriculumController(ICurriculumService service) : InstructorControllerBase
{
    [HttpGet("courses/{courseId:int}/curriculum")]
    public Task<CourseCurriculumDto> Get(int courseId) => service.GetAsync(courseId);

    [HttpPost("courses/{courseId:int}/sections")]
    public Task<CurriculumSectionDto> AddSection(int courseId, CurriculumSectionRequest request) => service.AddSectionAsync(courseId, request);

    [HttpPut("sections/{id:int}")]
    public async Task<IActionResult> UpdateSection(int id, CurriculumSectionRequest request)
    {
        await service.UpdateSectionAsync(id, request);
        return NoContent();
    }

    /// <summary>Moves the section one place up or down; returns the reordered curriculum.</summary>
    [HttpPut("sections/{id:int}/move")]
    public Task<CourseCurriculumDto> MoveSection(int id, MoveSectionRequest request) => service.MoveSectionAsync(id, request);

    /// <summary>What deleting the section would remove (for the confirmation message).</summary>
    [HttpGet("sections/{id:int}/delete-summary")]
    public Task<SectionDeleteSummaryDto> SectionDeleteSummary(int id) => service.GetSectionDeleteSummaryAsync(id);

    [HttpDelete("sections/{id:int}")]
    public async Task<IActionResult> DeleteSection(int id)
    {
        await service.DeleteSectionAsync(id);
        return NoContent();
    }

    public record VideoForm(string Title, IFormFile? Video);

    [HttpPost("sections/{id:int}/videos")]
    [RequestSizeLimit(500_000_000)]
    [RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)]
    public async Task<CurriculumVideoDto> UploadVideo(int id, [FromForm] VideoForm form, [FromServices] IValidator<VideoUploadRequest> validator)
    {
        var request = new VideoUploadRequest(form.Title ?? "", form.Video.ToFileUpload());
        await validator.ValidateAndThrowAsync(request);
        return await service.UploadVideoAsync(id, request);
    }

    /// <summary>Edits the title; the file is replaced only when a new one is sent.</summary>
    [HttpPut("videos/{id:int}")]
    [RequestSizeLimit(500_000_000)]
    [RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)]
    public async Task<CurriculumVideoDto> UpdateVideo(int id, [FromForm] VideoForm form, [FromServices] IValidator<VideoUpdateRequest> validator)
    {
        var request = new VideoUpdateRequest(form.Title ?? "", form.Video.ToFileUpload());
        await validator.ValidateAndThrowAsync(request);
        return await service.UpdateVideoAsync(id, request);
    }

    [HttpDelete("videos/{id:int}")]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        await service.DeleteVideoAsync(id);
        return NoContent();
    }

    [HttpPost("sections/{id:int}/references")]
    public Task<CurriculumReferenceDto> AddReference(int id, ReferenceRequest request) => service.AddReferenceAsync(id, request);

    [HttpPut("references/{id:int}")]
    public async Task<IActionResult> UpdateReference(int id, ReferenceRequest request)
    {
        await service.UpdateReferenceAsync(id, request);
        return NoContent();
    }

    [HttpDelete("references/{id:int}")]
    public async Task<IActionResult> DeleteReference(int id)
    {
        await service.DeleteReferenceAsync(id);
        return NoContent();
    }

    [HttpPost("sections/{id:int}/quizzes")]
    public Task<CurriculumExamDto> AddQuiz(int id, QuizRequest request) => service.AddQuizAsync(id, request);

    [HttpPut("quizzes/{id:int}")]
    public async Task<IActionResult> UpdateQuiz(int id, QuizRequest request)
    {
        await service.UpdateQuizAsync(id, request);
        return NoContent();
    }

    [HttpDelete("quizzes/{id:int}")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        await service.DeleteQuizAsync(id);
        return NoContent();
    }
}

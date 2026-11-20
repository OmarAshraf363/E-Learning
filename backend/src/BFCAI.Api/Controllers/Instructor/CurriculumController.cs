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

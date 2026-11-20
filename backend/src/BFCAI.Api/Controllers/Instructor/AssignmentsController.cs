using BFCAI.Api.Infrastructure;
using BFCAI.Application.Features.Instructor;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Instructor;

/// <summary>Legacy Instructor/AssinmentController.</summary>
[Route("api/instructor")]
public class AssignmentsController(IAssignmentService service, IValidator<AssignmentRequest> validator) : InstructorControllerBase
{
    public record AssignmentForm(string Title, string? Description, DateTime? DeadLine, IFormFile? Content);

    [HttpGet("courses/{courseId:int}/assignments")]
    public Task<List<SectionAssignmentsDto>> ByCourse(int courseId) => service.GetByCourseAsync(courseId);

    [HttpGet("assignments/{id:int}")] public Task<AssignmentDto> Get(int id) => service.GetAsync(id);

    [HttpPost("sections/{curriculumId:int}/assignments")]
    public async Task<AssignmentDto> Create(int curriculumId, [FromForm] AssignmentForm form)
        => await service.CreateAsync(curriculumId, await ToRequestAsync(form));

    [HttpPut("assignments/{id:int}")]
    public async Task<AssignmentDto> Update(int id, [FromForm] AssignmentForm form)
        => await service.UpdateAsync(id, await ToRequestAsync(form));

    [HttpDelete("assignments/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("assignments/{id:int}/submissions")]
    public Task<List<SubmissionDto>> Submissions(int id) => service.GetSubmissionsAsync(id);

    private async Task<AssignmentRequest> ToRequestAsync(AssignmentForm form)
    {
        var request = new AssignmentRequest(form.Title ?? "", form.Description, form.DeadLine, form.Content.ToFileUpload());
        await validator.ValidateAndThrowAsync(request);
        return request;
    }
}

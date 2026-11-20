using BFCAI.Api.Contracts;
using BFCAI.Application.Features.Courses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Legacy Admin/CourseController.</summary>
[Route("api/admin/courses")]
public class CoursesController(ICourseManagementService service, IValidator<CourseRequest> validator) : AdminControllerBase
{
    [HttpGet] public Task<List<AdminCourseRowDto>> GetAll() => service.GetAllAsync();
    [HttpGet("{id:int}")] public Task<CourseFormDto> Get(int id) => service.GetFormAsync(id);
    [HttpGet("{id:int}/details")] public Task<AdminCourseDetailsDto> Details(int id) => service.GetDetailsAsync(id);

    [HttpPost]
    public async Task<CourseFormDto> Create([FromForm] CourseForm form)
    {
        var request = form.ToRequest();
        await validator.ValidateAndThrowAsync(request);
        return await service.CreateAsync(request);
    }

    [HttpPut("{id:int}")]
    public async Task<CourseFormDto> Update(int id, [FromForm] CourseForm form)
    {
        var request = form.ToRequest();
        await validator.ValidateAndThrowAsync(request);
        return await service.UpdateAsync(id, request);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}

using BFCAI.Api.Infrastructure;
using BFCAI.Application.Features.Courses;
using BFCAI.Application.Features.Departments;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Legacy Admin/DepartmentController.</summary>
[Route("api/admin/departments")]
public class DepartmentsController(IDepartmentService service, IValidator<DepartmentRequest> validator) : AdminControllerBase
{
    public record DepartmentForm(string DepartmentName, string DepartmentDescription, IFormFile? Cover);

    [HttpGet] public Task<List<DepartmentListItemDto>> GetAll() => service.GetAllAsync();
    [HttpGet("{id:int}")] public Task<DepartmentDto> Get(int id) => service.GetAsync(id);
    [HttpGet("{id:int}/details")] public Task<DepartmentDetailsDto> Details(int id) => service.GetDetailsAsync(id);
    [HttpGet("{id:int}/courses")] public Task<List<AdminCourseRowDto>> Courses(int id) => service.GetCoursesAsync(id);

    [HttpPost]
    public async Task<DepartmentDto> Create([FromForm] DepartmentForm form)
        => await service.CreateAsync(await ToRequestAsync(form));

    [HttpPut("{id:int}")]
    public async Task<DepartmentDto> Update(int id, [FromForm] DepartmentForm form)
        => await service.UpdateAsync(id, await ToRequestAsync(form));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    private async Task<DepartmentRequest> ToRequestAsync(DepartmentForm form)
    {
        var request = new DepartmentRequest(form.DepartmentName ?? "", form.DepartmentDescription ?? "", form.Cover.ToFileUpload());
        await validator.ValidateAndThrowAsync(request);
        return request;
    }
}

using BFCAI.Application.Features.AcademicYears;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Legacy Admin/AcadmicYearController.</summary>
[Route("api/admin/academic-years")]
public class AcademicYearsController(IAcademicYearService service) : AdminControllerBase
{
    [HttpGet] public Task<List<AcademicYearDto>> GetAll() => service.GetAllAsync();
    [HttpGet("{id:int}")] public Task<AcademicYearDto> Get(int id) => service.GetAsync(id);
    [HttpPost] public Task<AcademicYearDto> Create(AcademicYearRequest request) => service.CreateAsync(request);
    [HttpPut("{id:int}")] public Task<AcademicYearDto> Update(int id, AcademicYearRequest request) => service.UpdateAsync(id, request);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}

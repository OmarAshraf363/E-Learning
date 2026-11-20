using BFCAI.Application.Features.Schedules;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Legacy Admin/ScheduleController.</summary>
[Route("api/admin/schedules")]
public class SchedulesController(IScheduleService service) : AdminControllerBase
{
    [HttpGet] public Task<List<ScheduleDto>> GetAll() => service.GetAllAsync();
    [HttpGet("{id:int}")] public Task<ScheduleFormDto> Get(int id) => service.GetAsync(id);
    [HttpPost] public Task<ScheduleFormDto> Create(ScheduleRequest request) => service.CreateAsync(request);
    [HttpPut("{id:int}")] public Task<ScheduleFormDto> Update(int id, ScheduleRequest request) => service.UpdateAsync(id, request);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}

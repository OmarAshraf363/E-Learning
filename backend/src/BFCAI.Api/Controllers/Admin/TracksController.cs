using BFCAI.Api.Infrastructure;
using BFCAI.Application.Features.Tracks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Legacy Admin/TrackController.</summary>
[Route("api/admin/tracks")]
public class TracksController(ITrackService service, IValidator<TrackRequest> validator) : AdminControllerBase
{
    public record TrackForm(string Name, string Description, IFormFile? Cover);

    [HttpGet] public Task<List<TrackListItemDto>> GetAll() => service.GetAllAsync();
    [HttpGet("{id:int}")] public Task<TrackDto> Get(int id) => service.GetAsync(id);
    [HttpGet("{id:int}/details")] public Task<TrackDetailsDto> Details(int id) => service.GetDetailsAsync(id);

    [HttpPost]
    public async Task<TrackDto> Create([FromForm] TrackForm form) => await service.CreateAsync(await ToRequestAsync(form));

    [HttpPut("{id:int}")]
    public async Task<TrackDto> Update(int id, [FromForm] TrackForm form) => await service.UpdateAsync(id, await ToRequestAsync(form));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    private async Task<TrackRequest> ToRequestAsync(TrackForm form)
    {
        var request = new TrackRequest(form.Name ?? "", form.Description ?? "", form.Cover.ToFileUpload());
        await validator.ValidateAndThrowAsync(request);
        return request;
    }
}

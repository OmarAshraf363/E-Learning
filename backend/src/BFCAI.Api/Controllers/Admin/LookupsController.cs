using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.Lookups;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Dropdown data for the admin forms.</summary>
[Route("api/admin/lookups")]
public class LookupsController(ILookupService service) : AdminControllerBase
{
    [HttpGet("departments")] public Task<List<LookupItem<int>>> Departments() => service.DepartmentsAsync();
    [HttpGet("tracks")] public Task<List<LookupItem<int>>> Tracks() => service.TracksAsync();
    [HttpGet("courses")] public Task<List<LookupItem<int>>> Courses() => service.CoursesAsync();
    [HttpGet("rooms")] public Task<List<LookupItem<int>>> Rooms() => service.RoomsAsync();
    [HttpGet("academic-years")] public Task<List<LookupItem<int>>> AcademicYears() => service.AcademicYearsAsync();
    [HttpGet("instructors")] public Task<List<LookupItem<string>>> Instructors() => service.InstructorsAsync();
    [HttpGet("users")] public Task<List<LookupItem<string>>> Users() => service.UsersAsync();
}

using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Public;

/// <summary>
/// Public storefront (legacy Customer/HomeController). Anonymous, but when a token is sent the
/// response is personalised (recommendations, enrolled courses) like the legacy pages.
/// </summary>
[ApiController]
[Route("api/catalog")]
[AllowAnonymous]
public class CatalogController(ICatalogService service) : ControllerBase
{
    [HttpGet("home")]
    public Task<HomeDto> Home([FromQuery] PaginationParams pagination) => service.GetHomeAsync(pagination);

    /// <summary>Home page category tabs. departmentId 0 / missing = all courses.</summary>
    [HttpGet("departments/courses")]
    public Task<DepartmentCoursesDto> CoursesByDepartment([FromQuery] int? departmentId, [FromQuery] PaginationParams pagination)
        => service.GetCoursesByDepartmentAsync(departmentId, pagination);

    [HttpGet("courses")]
    public Task<PagedResult<CourseCardDto>> Courses([FromQuery] CourseFilter filter, [FromQuery] PaginationParams pagination)
        => service.SearchCoursesAsync(filter, pagination);

    [HttpGet("courses/{id:int}")]
    public Task<CourseDetailsDto> CourseDetails(int id, [FromQuery] int? starFilter) => service.GetCourseDetailsAsync(id, starFilter);

    [HttpGet("categories")]
    public Task<List<DepartmentCardDto>> Categories() => service.GetCategoriesAsync();

    [HttpGet("categories/{id:int}")]
    public Task<CategoryDto> Category(int id) => service.GetCategoryAsync(id);

    [HttpGet("topics/{word}")]
    public Task<List<CourseCardDto>> Topic(string word) => service.GetTopicCoursesAsync(word);

    [HttpGet("tracks/{id:int}")]
    public Task<PublicTrackDto> Track(int id) => service.GetTrackAsync(id);

    [HttpGet("nav")]
    public Task<List<NavCategoryDto>> NavCategories() => service.GetNavCategoriesAsync();
}

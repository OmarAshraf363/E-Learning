using BFCAI.Api.Infrastructure;
using BFCAI.Application.Features.Courses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Legacy Admin/CourseController.</summary>
[Route("api/admin/courses")]
public class CoursesController(ICourseManagementService service, IValidator<CourseRequest> validator) : AdminControllerBase
{
    /// <summary>multipart/form-data body. Lists are sent as repeated keys (KeyWords=a&amp;KeyWords=b).</summary>
    public class CourseForm
    {
        public string CourseName { get; set; } = "";
        public string Description { get; set; } = "";
        public int Credits { get; set; }
        public decimal? Price { get; set; }
        public int? Rate { get; set; }
        public string? DemoVideoUrl { get; set; }
        public int? NumOfEnrollments { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? InstructorId { get; set; }
        public int? DepartmentId { get; set; }
        public int? TrackId { get; set; }
        public IFormFile? ImgCover { get; set; }
        public List<string>? KeyWords { get; set; }
        public List<string>? LearningObjectives { get; set; }
        public List<string>? TopicsCovered { get; set; }

        public CourseRequest ToRequest() => new(CourseName, Description, Credits, Price, Rate, DemoVideoUrl,
            NumOfEnrollments, StartDate, EndDate, InstructorId, DepartmentId, TrackId, ImgCover.ToFileUpload(),
            KeyWords, LearningObjectives, TopicsCovered);
    }

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

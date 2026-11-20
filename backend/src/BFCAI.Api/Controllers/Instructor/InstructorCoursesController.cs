using BFCAI.Api.Contracts;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Application.Features.Courses;
using BFCAI.Application.Features.Instructor;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Instructor;

/// <summary>Legacy Instructor/Dashboard + InstructorCourse controller.</summary>
[Route("api/instructor")]
public class InstructorCoursesController(
    IInstructorCourseService service, IValidator<CourseRequest> validator, ICurrentUser currentUser) : InstructorControllerBase
{
    [HttpGet("dashboard")] public Task<InstructorDashboardDto> Dashboard() => service.GetDashboardAsync();

    [HttpGet("courses")]
    public Task<PagedResult<InstructorCourseRowDto>> Courses([FromQuery] PaginationParams pagination) => service.GetMyCoursesAsync(pagination);

    /// <summary>The instructor's courses for dropdowns (exams, assignments, students pages).</summary>
    [HttpGet("courses/lookup")] public Task<List<LookupItem<int>>> CourseLookup() => service.GetMyCourseLookupAsync();

    [HttpGet("courses/form-lookups")] public Task<InstructorCourseLookupsDto> FormLookups() => service.GetFormLookupsAsync();

    [HttpGet("courses/{id:int}")] public Task<CourseFormDto> Get(int id) => service.GetFormAsync(id);

    [HttpPost("courses")]
    public async Task<CourseFormDto> Create([FromForm] CourseForm form)
    {
        var request = ToRequest(form);
        await validator.ValidateAndThrowAsync(request);
        return await service.CreateAsync(request);
    }

    [HttpPut("courses/{id:int}")]
    public async Task<CourseFormDto> Update(int id, [FromForm] CourseForm form)
    {
        var request = ToRequest(form);
        await validator.ValidateAndThrowAsync(request);
        return await service.UpdateAsync(id, request);
    }

    /// <summary>The instructor of the course is always the logged-in instructor.</summary>
    private CourseRequest ToRequest(CourseForm form) => form.ToRequest() with { InstructorId = currentUser.RequiredUserId };
}

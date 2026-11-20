using BFCAI.Application.Features.Student;
using BFCAI.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Student;

/// <summary>Legacy Student area. Course content requires an enrollment (checked in the service).</summary>
[ApiController]
[Route("api/student")]
[Authorize(Roles = Roles.Student)]
public class StudentController(IStudentService service) : ControllerBase
{
    [HttpGet("dashboard")] public Task<StudentDashboardDto> Dashboard() => service.GetDashboardAsync();
    [HttpGet("courses")] public Task<List<MyCourseDto>> MyCourses() => service.GetMyCoursesAsync();
    [HttpGet("courses/{id:int}")] public Task<CoursePlayerDto> CoursePlayer(int id) => service.GetCoursePlayerAsync(id);
    [HttpGet("courses/{id:int}/quick-access")] public Task<QuickAccessDto> QuickAccess(int id) => service.GetQuickAccessAsync(id);
    [HttpGet("sections/{id:int}")] public Task<SectionContentDto> Section(int id) => service.GetSectionContentAsync(id);
    [HttpGet("sections/{id:int}/quiz")] public Task<List<QuizQuestionDto>> Quiz(int id) => service.GetSectionQuizAsync(id);

    [HttpPost("feedback")]
    public async Task<IActionResult> AddFeedback(FeedbackRequest request) => Ok(new { id = await service.AddFeedbackAsync(request) });

    [HttpPut("feedback/{id:int}")]
    public async Task<IActionResult> UpdateFeedback(int id, FeedbackRequest request)
    {
        await service.UpdateFeedbackAsync(id, request);
        return NoContent();
    }

    [HttpDelete("feedback/{id:int}")]
    public async Task<IActionResult> DeleteFeedback(int id)
    {
        await service.DeleteFeedbackAsync(id);
        return NoContent();
    }

    [HttpPost("assignments/{id:int}/submissions")]
    public async Task<IActionResult> Submit(int id, SubmitAssignmentRequest request)
    {
        await service.SubmitAssignmentAsync(id, request);
        return NoContent();
    }
}

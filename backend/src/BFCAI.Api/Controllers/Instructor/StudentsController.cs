using BFCAI.Application.Features.Instructor;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Instructor;

/// <summary>Legacy Instructor/StudentController.</summary>
[Route("api/instructor")]
public class StudentsController(IInstructorStudentService service) : InstructorControllerBase
{
    [HttpGet("courses/{courseId:int}/students")]
    public Task<List<CourseStudentDto>> Students(int courseId) => service.GetStudentsAsync(courseId);

    [HttpPost("feedback")]
    public async Task<IActionResult> AddFeedback(StudentFeedbackRequest request)
    {
        await service.AddFeedbackAsync(request);
        return NoContent();
    }
}

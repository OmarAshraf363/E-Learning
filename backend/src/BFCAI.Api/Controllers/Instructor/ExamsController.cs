using BFCAI.Application.Features.Instructor;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Instructor;

/// <summary>Legacy Instructor/ExamController.</summary>
[Route("api/instructor")]
public class ExamsController(IExamService service) : InstructorControllerBase
{
    [HttpGet("exams")] public Task<List<ExamRowDto>> GetAll() => service.GetMyExamsAsync();
    [HttpGet("exams/{id:int}")] public Task<ExamDto> Get(int id) => service.GetAsync(id);
    [HttpPost("exams")] public Task<ExamDto> Create(ExamRequest request) => service.CreateAsync(request);
    [HttpPut("exams/{id:int}")] public Task<ExamDto> Update(int id, ExamRequest request) => service.UpdateAsync(id, request);

    [HttpDelete("exams/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("exams/{id:int}/questions")] public Task<ExamQuestionsDto> Questions(int id) => service.GetQuestionsAsync(id);
    [HttpPost("exams/{id:int}/questions")] public Task<QuestionDto> AddQuestion(int id, QuestionRequest request) => service.AddQuestionAsync(id, request);
    [HttpGet("questions/{id:int}")] public Task<QuestionDto> GetQuestion(int id) => service.GetQuestionAsync(id);
    [HttpPut("questions/{id:int}")] public Task<QuestionDto> UpdateQuestion(int id, QuestionRequest request) => service.UpdateQuestionAsync(id, request);
}

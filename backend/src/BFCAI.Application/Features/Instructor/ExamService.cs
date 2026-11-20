using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Entities;
using FluentValidation;

namespace BFCAI.Application.Features.Instructor;

public record ExamRowDto(int Id, string Title, string? CourseName, DateTime ExamDate, int TotalQuestions);
public record ExamDto(int Id, string Title, DateTime ExamDate, int CourseId);
public record ExamRequest(string Title, DateTime? ExamDate, int CourseId);

public record ChoiceDto(int Id, string ChoiceText, bool IsCorrect);
public record QuestionDto(int Id, string QuestionText, int ExamId, List<ChoiceDto> Choices);
public record ExamQuestionsDto(int ExamId, string ExamTitle, List<QuestionDto> Questions);

public record ChoiceRequest(string ChoiceText, bool IsCorrect);
/// <summary>Legacy QuestionVM. On edit the choices replace the existing ones.</summary>
public record QuestionRequest(string QuestionText, List<ChoiceRequest>? Choices);

public class ExamRequestValidator : AbstractValidator<ExamRequest>
{
    public ExamRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ExamDate).NotNull();
        RuleFor(x => x.CourseId).GreaterThan(0).WithMessage("Please select a course.");
    }
}

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator() => RuleFor(x => x.QuestionText).NotEmpty();
}

/// <summary>Legacy Instructor/ExamController (exams + question builder).</summary>
public interface IExamService
{
    Task<List<ExamRowDto>> GetMyExamsAsync();
    Task<ExamDto> GetAsync(int id);
    Task<ExamDto> CreateAsync(ExamRequest request);
    Task<ExamDto> UpdateAsync(int id, ExamRequest request);
    Task DeleteAsync(int id);
    Task<ExamQuestionsDto> GetQuestionsAsync(int examId);
    Task<QuestionDto> GetQuestionAsync(int questionId);
    Task<QuestionDto> AddQuestionAsync(int examId, QuestionRequest request);
    Task<QuestionDto> UpdateQuestionAsync(int questionId, QuestionRequest request);
}

public class ExamService(IUnitOfWork unitOfWork, InstructorAccess access) : IExamService
{
    private IGenericRepository<Exam> Exams => unitOfWork.Repository<Exam>();
    private IGenericRepository<Question> Questions => unitOfWork.Repository<Question>();

    public Task<List<ExamRowDto>> GetMyExamsAsync()
    {
        var me = access.InstructorId;
        return Exams.SelectAsync(e => new ExamRowDto(e.ExamID, e.Title, e.Course != null ? e.Course.CourseName : null,
            e.ExamDate, e.Questions.Count), e => e.InstructorId == me);
    }

    public async Task<ExamDto> GetAsync(int id)
    {
        await access.EnsureOwnsExamAsync(id);
        return (await Exams.SelectOneAsync(e => e.ExamID == id, e => new ExamDto(e.ExamID, e.Title, e.ExamDate, e.CourseID)))!;
    }

    public async Task<ExamDto> CreateAsync(ExamRequest request)
    {
        await access.EnsureOwnsCourseAsync(request.CourseId);
        var exam = new Exam { Title = request.Title, ExamDate = request.ExamDate!.Value, CourseID = request.CourseId, InstructorId = access.InstructorId };
        await Exams.AddAsync(exam);
        await unitOfWork.SaveChangesAsync();
        return new ExamDto(exam.ExamID, exam.Title, exam.ExamDate, exam.CourseID);
    }

    public async Task<ExamDto> UpdateAsync(int id, ExamRequest request)
    {
        await access.EnsureOwnsExamAsync(id);
        await access.EnsureOwnsCourseAsync(request.CourseId);
        var exam = (await Exams.GetOneAsync(e => e.ExamID == id, tracked: true))!;
        exam.Title = request.Title;
        exam.ExamDate = request.ExamDate!.Value;
        exam.CourseID = request.CourseId;
        await unitOfWork.SaveChangesAsync();
        return new ExamDto(exam.ExamID, exam.Title, exam.ExamDate, exam.CourseID);
    }

    /// <summary>The legacy exams page linked to a missing "Delete" action; DeleteQuiz did the same job.</summary>
    public async Task DeleteAsync(int id)
    {
        await access.EnsureOwnsExamAsync(id);
        var exam = (await Exams.GetOneAsync(e => e.ExamID == id, tracked: true))!;
        Exams.Delete(exam);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<ExamQuestionsDto> GetQuestionsAsync(int examId)
    {
        await access.EnsureOwnsExamAsync(examId);
        return (await Exams.SelectOneAsync(e => e.ExamID == examId, e => new ExamQuestionsDto(e.ExamID, e.Title,
            e.Questions.Select(q => new QuestionDto(q.QuestionID, q.QuestionText, q.ExamID,
                q.Choices.Select(c => new ChoiceDto(c.QuestionChoiceID, c.ChoiceText, c.IsCorrect)).ToList())).ToList())))!;
    }

    public async Task<QuestionDto> GetQuestionAsync(int questionId)
    {
        var examId = await ExamIdOfAsync(questionId);
        await access.EnsureOwnsExamAsync(examId);
        return await ToDtoAsync(questionId);
    }

    public async Task<QuestionDto> AddQuestionAsync(int examId, QuestionRequest request)
    {
        await access.EnsureOwnsExamAsync(examId);
        var question = new Question { ExamID = examId, QuestionText = request.QuestionText, Choices = ToChoices(request) };
        await Questions.AddAsync(question);
        await unitOfWork.SaveChangesAsync();
        return await ToDtoAsync(question.QuestionID);
    }

    /// <summary>Legacy: update the text, delete the old choices, insert the new ones.</summary>
    public async Task<QuestionDto> UpdateQuestionAsync(int questionId, QuestionRequest request)
    {
        await access.EnsureOwnsExamAsync(await ExamIdOfAsync(questionId));
        var question = (await Questions.GetOneAsync(q => q.QuestionID == questionId, tracked: true, q => q.Choices))!;

        question.QuestionText = request.QuestionText;
        unitOfWork.Repository<QuestionChoice>().DeleteRange(question.Choices);
        question.Choices = ToChoices(request);

        await unitOfWork.SaveChangesAsync();
        return await ToDtoAsync(questionId);
    }

    private async Task<int> ExamIdOfAsync(int questionId)
        => await Questions.SelectOneAsync(q => q.QuestionID == questionId, q => (int?)q.ExamID)
           ?? throw new NotFoundException(nameof(Question), questionId);

    private static List<QuestionChoice> ToChoices(QuestionRequest request)
        => (request.Choices ?? []).Where(c => !string.IsNullOrWhiteSpace(c.ChoiceText))
            .Select(c => new QuestionChoice { ChoiceText = c.ChoiceText.Trim(), IsCorrect = c.IsCorrect }).ToList();

    private async Task<QuestionDto> ToDtoAsync(int questionId)
        => (await Questions.SelectOneAsync(q => q.QuestionID == questionId, q => new QuestionDto(q.QuestionID, q.QuestionText, q.ExamID,
            q.Choices.Select(c => new ChoiceDto(c.QuestionChoiceID, c.ChoiceText, c.IsCorrect)).ToList())))!;
}

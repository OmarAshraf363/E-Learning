using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BFCAI.Application.Features.Enrollments;

public record EnrollmentRowDto(int Id, string? StudentEmail, string? CourseName);

/// <summary>Legacy Admin/Enrollment/Index: latest 10 enrollments + total.</summary>
public record EnrollmentOverviewDto(int TotalEnrollments, List<EnrollmentRowDto> Latest);

public record EnrollByEmailRequest(string Email, int CourseId);

public record CsvImportResultDto(int Added, int Skipped);

public class EnrollByEmailRequestValidator : AbstractValidator<EnrollByEmailRequest>
{
    public EnrollByEmailRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.CourseId).GreaterThan(0).WithMessage("Please select a course.");
    }
}

/// <summary>Legacy Admin/EnrollmentController.</summary>
public interface IEnrollmentService
{
    Task<EnrollmentOverviewDto> GetOverviewAsync();
    Task EnrollByEmailAsync(EnrollByEmailRequest request);
    Task<CsvImportResultDto> ImportCsvAsync(FileUpload file);
}

public class EnrollmentService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : IEnrollmentService
{
    private IGenericRepository<Enrollment> Enrollments => unitOfWork.Repository<Enrollment>();

    public async Task<EnrollmentOverviewDto> GetOverviewAsync()
    {
        var latest = await Enrollments.SelectAsync(
            e => new EnrollmentRowDto(e.EnrollmentID, e.Student != null ? e.Student.Email : null,
                e.Course != null ? e.Course.CourseName : null),
            orderBy: q => q.OrderByDescending(e => e.EnrollmentID),
            take: 10);
        return new EnrollmentOverviewDto(await Enrollments.CountAsync(), latest);
    }

    public async Task EnrollByEmailAsync(EnrollByEmailRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
                   ?? throw new BadRequestException("Student with this email does not exist.");

        if (!await unitOfWork.Repository<Course>().AnyAsync(c => c.CourseID == request.CourseId))
            throw new NotFoundException(nameof(Course), request.CourseId);

        await Enrollments.AddAsync(new Enrollment { CourseID = request.CourseId, StudentId = user.Id });
        await unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Each line: "studentId,courseId" (legacy format). The first value may also be the student's
    /// email, as the legacy upload dialog described. Invalid lines are skipped instead of failing
    /// the whole upload (legacy crashed on int.Parse).
    /// </summary>
    public async Task<CsvImportResultDto> ImportCsvAsync(FileUpload file)
    {
        int added = 0, skipped = 0;
        using var reader = new StreamReader(file.Content);

        while (await reader.ReadLineAsync() is { } line)
        {
            var fields = line.Split(',');
            if (fields.Length < 2 || !int.TryParse(fields[1].Trim(), out var courseId))
            {
                if (!string.IsNullOrWhiteSpace(line)) skipped++;
                continue;
            }

            var key = fields[0].Trim();
            var student = key.Contains('@') ? await userManager.FindByEmailAsync(key) : await userManager.FindByIdAsync(key);
            var courseExists = await unitOfWork.Repository<Course>().AnyAsync(c => c.CourseID == courseId);

            if (student is null || !courseExists)
            {
                skipped++;
                continue;
            }

            await Enrollments.AddAsync(new Enrollment { StudentId = student.Id, CourseID = courseId });
            added++;
        }

        await unitOfWork.SaveChangesAsync();
        return new CsvImportResultDto(added, skipped);
    }
}

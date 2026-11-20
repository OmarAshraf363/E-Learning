using BFCAI.Api.Infrastructure;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Features.Enrollments;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Admin;

/// <summary>Legacy Admin/EnrollmentController.</summary>
[Route("api/admin/enrollments")]
public class EnrollmentsController(IEnrollmentService service) : AdminControllerBase
{
    [HttpGet] public Task<EnrollmentOverviewDto> Overview() => service.GetOverviewAsync();

    [HttpPost("by-email")]
    public async Task<IActionResult> EnrollByEmail(EnrollByEmailRequest request)
    {
        await service.EnrollByEmailAsync(request);
        return NoContent();
    }

    [HttpPost("csv")]
    public Task<CsvImportResultDto> UploadCsv(IFormFile? csvFile)
        => service.ImportCsvAsync(csvFile.ToFileUpload() ?? throw new BadRequestException("Please choose a CSV file."));
}

using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Entities;
using FluentValidation;

namespace BFCAI.Application.Features.AcademicYears;

public record AcademicYearDto(int Id, string YearName);

public record AcademicYearRequest(string YearName);

public class AcademicYearRequestValidator : AbstractValidator<AcademicYearRequest>
{
    public AcademicYearRequestValidator() => RuleFor(x => x.YearName).NotEmpty();
}

/// <summary>Legacy Admin/AcadmicYearController.</summary>
public interface IAcademicYearService
{
    Task<List<AcademicYearDto>> GetAllAsync();
    Task<AcademicYearDto> GetAsync(int id);
    Task<AcademicYearDto> CreateAsync(AcademicYearRequest request);
    Task<AcademicYearDto> UpdateAsync(int id, AcademicYearRequest request);
    Task DeleteAsync(int id);
}

public class AcademicYearService(IUnitOfWork unitOfWork) : IAcademicYearService
{
    private IGenericRepository<AcademicYear> Years => unitOfWork.Repository<AcademicYear>();

    public Task<List<AcademicYearDto>> GetAllAsync()
        => Years.SelectAsync(y => new AcademicYearDto(y.AcademicYearID, y.YearName));

    public async Task<AcademicYearDto> GetAsync(int id)
        => await Years.SelectOneAsync(y => y.AcademicYearID == id, y => new AcademicYearDto(y.AcademicYearID, y.YearName))
           ?? throw new NotFoundException("Academic year", id);

    // Technical fix: the legacy AJAX save returned before Commit(), so nothing was saved.
    public async Task<AcademicYearDto> CreateAsync(AcademicYearRequest request)
    {
        var year = new AcademicYear { YearName = request.YearName };
        await Years.AddAsync(year);
        await unitOfWork.SaveChangesAsync();
        return new AcademicYearDto(year.AcademicYearID, year.YearName);
    }

    public async Task<AcademicYearDto> UpdateAsync(int id, AcademicYearRequest request)
    {
        var year = await Years.GetOneAsync(y => y.AcademicYearID == id, tracked: true)
                   ?? throw new NotFoundException("Academic year", id);
        year.YearName = request.YearName;
        await unitOfWork.SaveChangesAsync();
        return new AcademicYearDto(year.AcademicYearID, year.YearName);
    }

    public async Task DeleteAsync(int id)
    {
        var year = await Years.GetOneAsync(y => y.AcademicYearID == id, tracked: true)
                   ?? throw new NotFoundException("Academic year", id);
        Years.Delete(year);
        await unitOfWork.SaveChangesAsync();
    }
}

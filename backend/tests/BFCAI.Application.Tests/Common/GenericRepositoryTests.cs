using BFCAI.Application.Common.Models;
using BFCAI.Domain.Entities;

namespace BFCAI.Application.Tests.Common;

public class GenericRepositoryTests
{
    [Fact]
    public async Task Seed_data_is_created()
    {
        using var db = new TestDb();
        var departments = await db.UnitOfWork.Repository<Department>().GetAllAsync();
        Assert.Equal(2, departments.Count);
    }

    [Fact]
    public async Task GetPagedAsync_returns_requested_page_and_total()
    {
        using var db = new TestDb();
        var repo = db.UnitOfWork.Repository<AcademicYear>();

        var page = await repo.GetPagedAsync(y => y.YearName,
            new PaginationParams { Page = 2, PageSize = 3 },
            orderBy: q => q.OrderBy(y => y.AcademicYearID));

        Assert.Equal(4, page.TotalCount);
        Assert.Equal(2, page.TotalPages);
        Assert.Equal(["2023-2024"], page.Items);
    }

    [Fact]
    public async Task Changes_are_saved_only_on_SaveChanges()
    {
        using var db = new TestDb();
        var repo = db.UnitOfWork.Repository<Room>();

        await repo.AddAsync(new Room { Name = "Lab 303", Capacity = 25 });
        Assert.Equal(2, await repo.CountAsync());

        await db.UnitOfWork.SaveChangesAsync();
        Assert.Equal(3, await repo.CountAsync());
    }
}

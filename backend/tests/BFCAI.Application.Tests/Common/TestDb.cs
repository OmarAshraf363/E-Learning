using BFCAI.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BFCAI.Application.Tests.Common;

/// <summary>
/// Real EF Core context on an in-memory SQLite database (schema + seed data created).
/// Services are tested against real repositories instead of mocks.
/// </summary>
public sealed class TestDb : IDisposable
{
    private readonly SqliteConnection _connection;
    public AppDbContext Context { get; }
    public UnitOfWork UnitOfWork { get; }

    public TestDb()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options;
        Context = new AppDbContext(options);
        Context.Database.EnsureCreated();
        UnitOfWork = new UnitOfWork(Context);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}

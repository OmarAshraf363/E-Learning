using System.Collections.Concurrent;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Infrastructure.Persistence.Repositories;

namespace BFCAI.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public IGenericRepository<T> Repository<T>() where T : class
        => (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new GenericRepository<T>(context));

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}

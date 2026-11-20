namespace BFCAI.Application.Common.Interfaces;

/// <summary>
/// Single entry point to the data layer. Use <see cref="Repository{T}"/> for simple CRUD;
/// specific repositories are exposed as properties when an entity needs custom queries.
/// Nothing is persisted until <see cref="SaveChangesAsync"/> is called.
/// </summary>
public interface IUnitOfWork
{
    IGenericRepository<T> Repository<T>() where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

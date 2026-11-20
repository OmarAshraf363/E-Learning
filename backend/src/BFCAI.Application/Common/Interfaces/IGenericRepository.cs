using System.Linq.Expressions;
using BFCAI.Application.Common.Models;

namespace BFCAI.Application.Common.Interfaces;

/// <summary>
/// Generic data access for any entity. Reads are NOT tracked unless <c>tracked: true</c>
/// is passed (pass it when you load an entity in order to modify it).
/// Entity-specific queries (complex projections) live in specific repositories.
/// </summary>
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetOneAsync(
        Expression<Func<T, bool>> filter,
        bool tracked = false,
        params Expression<Func<T, object?>>[] includes);

    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool tracked = false,
        params Expression<Func<T, object?>>[] includes);

    /// <summary>Projects rows straight into a DTO (only the selected columns are queried).</summary>
    Task<List<TResult>> SelectAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? take = null);

    Task<TResult?> SelectOneAsync<TResult>(
        Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> selector);

    Task<PagedResult<TResult>> GetPagedAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        PaginationParams pagination,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}

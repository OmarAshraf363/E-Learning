using System.Linq.Expressions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BFCAI.Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext Context = context;
    protected readonly DbSet<T> Set = context.Set<T>();

    protected IQueryable<T> Query(bool tracked, Expression<Func<T, object?>>[] includes)
    {
        IQueryable<T> query = tracked ? Set : Set.AsNoTracking();
        foreach (var include in includes)
            query = query.Include(include);
        return query;
    }

    public Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, bool tracked = false,
        params Expression<Func<T, object?>>[] includes)
        => Query(tracked, includes).FirstOrDefaultAsync(filter);

    public Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracked = false,
        params Expression<Func<T, object?>>[] includes)
    {
        var query = Query(tracked, includes);
        if (filter is not null) query = query.Where(filter);
        if (orderBy is not null) query = orderBy(query);
        return query.ToListAsync();
    }

    public Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? take = null)
    {
        IQueryable<T> query = Set.AsNoTracking();
        if (filter is not null) query = query.Where(filter);
        if (orderBy is not null) query = orderBy(query);
        if (take is not null) query = query.Take(take.Value);
        return query.Select(selector).ToListAsync();
    }

    public Task<TResult?> SelectOneAsync<TResult>(Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> selector)
        => Set.AsNoTracking().Where(filter).Select(selector).FirstOrDefaultAsync();

    public async Task<PagedResult<TResult>> GetPagedAsync<TResult>(Expression<Func<T, TResult>> selector,
        PaginationParams pagination, Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
    {
        IQueryable<T> query = Set.AsNoTracking();
        if (filter is not null) query = query.Where(filter);
        if (orderBy is not null) query = orderBy(query);
        return await query.Select(selector).ToPagedResultAsync(pagination);
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> filter) => Set.AnyAsync(filter);

    public Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        => filter is null ? Set.CountAsync() : Set.CountAsync(filter);

    public async Task AddAsync(T entity) => await Set.AddAsync(entity);
    public Task AddRangeAsync(IEnumerable<T> entities) => Set.AddRangeAsync(entities);
    public void Update(T entity) => Set.Update(entity);
    public void Delete(T entity) => Set.Remove(entity);
    public void DeleteRange(IEnumerable<T> entities) => Set.RemoveRange(entities);
}

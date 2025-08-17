using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System;
using Banha_UniverCity.Data;
using Banha_UniverCity.Repository.IRepository;

namespace DataAccess.Repository.ModelsRepository
{
    public class GenralRepository<T> : IGenralRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<T> dbSet;

        public GenralRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet.AsNoTracking();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }
        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
        }
        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        public T? GetOne(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties)
        {
            return Get(expression, includeProperties).FirstOrDefault();
        }
        public void Create(T entity)
        {
            dbSet.Add(entity);

        }

        public void Delete(T entity)
        {

            dbSet.Remove(entity);


        }
        public void Edit(T entity)
        {
            dbSet.Update(entity);

        }

        public T GetOneWithNoTrack(Expression<Func<T, bool>> expression)
        {
           return dbSet.Where(expression).AsNoTracking().FirstOrDefault();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool asNoTracking = true,
        params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>> filter,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            query = query.Where(filter);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            dbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }
    }
}

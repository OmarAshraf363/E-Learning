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

        public IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
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
    }
}

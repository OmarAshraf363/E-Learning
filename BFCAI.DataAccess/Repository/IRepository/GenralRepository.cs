using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System;
using Banha_UniverCity.Data;

namespace Banha_UniverCity.Repository.IRepository
{
    public class GenralRepository<T>:IGenralRepository<T> where T : class
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
    }
}

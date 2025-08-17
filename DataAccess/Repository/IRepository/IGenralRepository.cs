using System.Linq.Expressions;

namespace Banha_UniverCity.Repository.IRepository
{
    public interface IGenralRepository<T> where T : class
    {
        IQueryable<T> Get(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includeProperties);
        T? GetOne(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties);
        void Edit(T entity);
        void Create(T entity);
        void Delete(T entity);
        T? GetOneWithNoTrack(Expression<Func<T, bool>> expression);
        void AddRange(IEnumerable<T> entities);
        void UpdateRange(IEnumerable<T> entities);
        void DeleteRange(IEnumerable<T> entities);

        Task<IReadOnlyList<T>> GetAllAsync(
               Expression<Func<T, bool>>? filter = null,
               Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
               bool asNoTracking = true,
               params Expression<Func<T, object>>[] includes);

        Task<T?> GetOneAsync(
            Expression<Func<T, bool>> filter,
            bool asNoTracking = true,
            params Expression<Func<T, object>>[] includes);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task DeleteRangeAsync(IEnumerable<T> entities);



    }
}

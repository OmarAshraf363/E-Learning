using System.Linq.Expressions;

namespace Banha_UniverCity.Repository.IRepository
{
    public interface IGenralRepository<T> where T : class
    {
        IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includeProperties);
        T? GetOne(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties);
        void Edit(T entity);
        void Create(T entity);
        void Delete(T entity);
        T? GetOneWithNoTrack(Expression<Func<T, bool>> expression);
        void AddRange(IEnumerable<T> entities);
        void UpdateRange(IEnumerable<T> entities);
        void DeleteRange(IEnumerable<T> entities);
    }
}

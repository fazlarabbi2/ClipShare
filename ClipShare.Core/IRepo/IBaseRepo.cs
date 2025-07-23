using ClipShare.Entities;
using System.Linq.Expressions;

namespace ClipShare.Core.IRepo
{
    public interface IBaseRepo<T> where T : BaseEntity
    {
        void Add(T entity);
        void Update(T source, T destination);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> criteria);
        Task<T> GetByIdAsync(int id, string includeProperties = null);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> criteria, string includeProperties = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> criteria = null, string includeProperties = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderedBy = null);
        Task<int> countAsync(Expression<Func<T, bool>> criteria = null);
    }
}

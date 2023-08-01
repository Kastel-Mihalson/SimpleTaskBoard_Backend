using System.Linq.Expressions;

namespace SimpleTaskBoard.Infrastructure.Interfaces
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> condition);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

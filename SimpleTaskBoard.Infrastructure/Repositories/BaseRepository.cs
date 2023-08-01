using Microsoft.EntityFrameworkCore;
using SimpleTaskBoard.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace SimpleTaskBoard.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected SimpleTaskBoardDbContext DbContext { get; set; }

        public BaseRepository(SimpleTaskBoardDbContext context)
        {
            DbContext = context;
        }

        public IQueryable<T> GetAll()
        {
            return DbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> condition)
        {
            return DbContext.Set<T>().Where(condition).AsNoTracking();
        }

        public void Create(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            DbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }
    }
}

namespace Auth.API.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetById(Guid id);

        void Create(T entity);

        void Update(T entity);

        T Delete(Guid id);
    }
}

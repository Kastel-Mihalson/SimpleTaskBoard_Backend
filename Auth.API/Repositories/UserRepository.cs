using Auth.API.Models;

namespace Auth.API.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly AuthContext _dbContext;

        public UserRepository(AuthContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(User entity)
        {
            _dbContext.Users.Add(entity);
            _dbContext.SaveChanges();
        }

        public User? Delete(Guid id)
        {
            var item = GetById(id);

            if (item != null)
            {
                _dbContext.Users.Remove(item);
                _dbContext.SaveChanges();
            }

            return item;
        }

        public IEnumerable<User> GetAll()
        {
            return _dbContext.Users;
        }

        public User? GetById(Guid id)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public void Update(User entity)
        {
            _dbContext.Users.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}

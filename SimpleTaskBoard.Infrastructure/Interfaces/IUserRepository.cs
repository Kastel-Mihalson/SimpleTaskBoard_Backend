using SimpleTaskBoard.Domain.Models;

namespace SimpleTaskBoard.Infrastructure.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IReadOnlyList<User>> GetAllUsers();
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByEmail(string email);
    }
}

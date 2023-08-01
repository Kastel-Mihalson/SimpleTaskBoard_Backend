using Microsoft.EntityFrameworkCore;
using SimpleTaskBoard.Domain.Models;
using SimpleTaskBoard.Infrastructure.Interfaces;

namespace SimpleTaskBoard.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(SimpleTaskBoardDbContext dbContext) : base(dbContext) { }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await GetByCondition(user => user.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await GetByCondition(user => user.Id.Equals(id)).FirstOrDefaultAsync();
        }
    }
}

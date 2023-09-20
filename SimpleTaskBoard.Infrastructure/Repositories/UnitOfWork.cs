using SimpleTaskBoard.Infrastructure.Interfaces;

namespace SimpleTaskBoard.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SimpleTaskBoardDbContext _dbContext;
        private IUserRepository _user;
        private IBookRepository _book;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_dbContext);
                }

                return _user;
            }
        }

        public IBookRepository Book
        {
            get
            {
                if (_book == null)
                {
                    _book = new BookRepository(_dbContext);
                }

                return _book;
            }
        }

        public UnitOfWork(SimpleTaskBoardDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

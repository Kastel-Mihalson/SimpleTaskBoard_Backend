using Microsoft.EntityFrameworkCore;
using SimpleTaskBoard.Domain.Models;
using SimpleTaskBoard.Infrastructure.Interfaces;

namespace SimpleTaskBoard.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(SimpleTaskBoardDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Book>> GetAllBooks()
        {
            return await GetAll().OrderBy(b => b.Title).ToListAsync();
        }
    }
}

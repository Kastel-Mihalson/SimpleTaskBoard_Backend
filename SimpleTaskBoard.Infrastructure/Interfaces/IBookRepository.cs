using SimpleTaskBoard.Domain.Models;

namespace SimpleTaskBoard.Infrastructure.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<IReadOnlyList<Book>> GetAllBooks();
    }
}

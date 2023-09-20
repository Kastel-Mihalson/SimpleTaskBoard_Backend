namespace SimpleTaskBoard.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }

        IBookRepository Book { get; }

        Task SaveAsync();
    }
}

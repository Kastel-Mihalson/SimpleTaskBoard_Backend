namespace SimpleTaskBoard.Infrastructure.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }

        IBookRepository Book { get; }

        Task SaveAsync();
    }
}

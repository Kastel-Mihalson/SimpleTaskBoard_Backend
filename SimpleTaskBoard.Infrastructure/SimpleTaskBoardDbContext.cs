using Microsoft.EntityFrameworkCore;
using SimpleTaskBoard.Domain.Models;

namespace SimpleTaskBoard.Infrastructure
{
    public class SimpleTaskBoardDbContext : DbContext
    {
        public SimpleTaskBoardDbContext(DbContextOptions<SimpleTaskBoardDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}

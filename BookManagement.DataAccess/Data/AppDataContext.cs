using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace BookManagementAPI.Data
{
    public class AppDataContext:DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }
        public DbSet<Books> Books {get;set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Books>().HasData(
            new Books{Id = 1, AuthorName = "Harry Poter", Title = "Harry Poter And The Grinch", PublicationYear = 2000 },
            new Books{Id = 2, AuthorName = "Henry Sirius", Title = "The Ring", PublicationYear = 1999},
            new Books{Id = 3, AuthorName = "The Bottle", Title = "The Laptop And Its Adventures", PublicationYear = 2025}
            );
        }
    }
}

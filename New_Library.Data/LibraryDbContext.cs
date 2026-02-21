using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data.Models;

namespace New_Web_Library.Data
{
    
    public class LibraryDbContext:IdentityDbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        :base(options)
        {
            
        }

        public virtual DbSet<User> Users { get; set; } = null!;

        public virtual DbSet<Book> Books { get; set; } = null!;

        public virtual DbSet<UserBook> UsersBooks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);

        }

    }
}

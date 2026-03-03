using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using New_Web_Library.Data.Models;

namespace New_Web_Library.Data
{
    using static New_Web_Library.GCommon.EntityValidations.Users;
    
    public class LibraryDbContext:IdentityDbContext<User,IdentityRole<Guid>,Guid>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        :base(options)
        {
            
        }

       
        public virtual DbSet<Book> Books { get; set; } = null!;

        public virtual DbSet<UserBook> UsersBooks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            
            
            base.OnModelCreating(modelBuilder);
          
            
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(EmailAddressMaxLength);

            modelBuilder.Entity<User>().Property(u => u.PhoneNumber).IsRequired().HasMaxLength(PhoneNumberMaxLength);

            modelBuilder.Entity<User>().HasIndex(u => u.NormalizedEmail).IsUnique();



            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);

        }


    }
}

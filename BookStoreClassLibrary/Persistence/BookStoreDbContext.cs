using BookStoreClassLibrary.Core.Enums;
using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookStoreClassLibrary.Core.Security;

namespace BookStoreClassLibrary.Persistence
{
    public class BookStoreDbContext : IdentityDbContext
    {
        public BookStoreDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Role.ADMINISTRATOR.GetDescription(), NormalizedName = Role.ADMINISTRATOR.GetDescription().ToUpper()});
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Role.CUSTOMER.GetDescription(), NormalizedName = Role.CUSTOMER.GetDescription().ToUpper()});
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Role.LIBRARIAN.GetDescription(), NormalizedName = Role.LIBRARIAN.GetDescription().ToUpper()});
        }
    }
}

using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.Repository;
using BookStoreClassLibrary.Core.Security;
using Microsoft.EntityFrameworkCore;

namespace BookStoreClassLibrary.Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _dbContext;
        public IGenericRepository<Author, Guid> AuthorRepository { get; set; }
        public IGenericRepository<Book, Guid> BookRepository { get; set; }
        public IGenericRepository<RefreshToken, Guid> RefreshTokenRepository { get; set; }

        public UnitOfWork(DbContext dbContext, IGenericRepository<Author, Guid> authorRepository, 
                                               IGenericRepository<Book, Guid> bookRepository,
                                               IGenericRepository<RefreshToken, Guid> refreshTokenRepository)
        {
            _dbContext = dbContext;
            AuthorRepository = authorRepository;
            BookRepository = bookRepository;
            RefreshTokenRepository = refreshTokenRepository;
        }

        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

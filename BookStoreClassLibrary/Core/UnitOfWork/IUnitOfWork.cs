using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.Repository;
using BookStoreClassLibrary.Core.Security;

namespace BookStoreClassLibrary.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Author, Guid> AuthorRepository { get; }
        public IGenericRepository<Book, Guid> BookRepository { get; }
        public IGenericRepository<RefreshToken, Guid> RefreshTokenRepository { get; }
        Task CompleteAsync();
    }
}

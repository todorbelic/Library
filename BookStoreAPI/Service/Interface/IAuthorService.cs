using BookStoreAPI.DTO.Author;
using BookStoreAPI.DTO.Book;
using BookStoreClassLibrary.Core.Entities;
using System.Linq.Expressions;

namespace BookStoreAPI.Service.Interface
{
    public interface IAuthorService
    {
        Task Create(CreateAuthorDTO author);
        Task Update(AuthorDTO author);
        Task<AuthorPreviewDTO> GetById(Guid id);
        List<AuthorDTO> GetAll();
        Task DeleteById(Guid id);
        Task<Author> GetEntityById(Guid id, params Expression<Func<Author, object>>[] includes);
        Task<List<BookDTO>> GetAllBooksByAuthorId(Guid id);
    }
}

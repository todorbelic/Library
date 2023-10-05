using BookStoreAPI.DTO.Book;
using BookStoreClassLibrary.Core.Entities;
using System.Linq.Expressions;

namespace BookStoreAPI.Service.Interface
{
    public interface IBookService
    {
        Task Create(CreateBookDTO book);
        Task Update(BookDTO author);
        Task<BookPreviewDTO> GetById(Guid id);
        Task<Book> GetEntityById(Guid id, params Expression<Func<Book, object>>[] includes);
        List<BookDTO> GetAll();
        Task DeleteById(Guid id);
        Task AddAuthorById(BookAuthorDTO bookAuthorDTO);
        Task RemoveAuthorById(BookAuthorDTO bookAuthorDTO);
    }
}

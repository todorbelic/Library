using AutoMapper;
using BookStoreAPI.DTO.Book;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Service.Interface;
using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStoreAPI.Service.Implementation
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthorService authorService;
        private IMapper mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper, IAuthorService authorService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.authorService = authorService;
        }

        public async Task Create(CreateBookDTO bookDTO)
        {
            var book = mapper.Map<Book>(bookDTO);
            foreach (var authorId in bookDTO.AuthorIds)
            {
                book.Authors.Add(await authorService.GetEntityById(authorId));
            }
            await unitOfWork.BookRepository.Add(book);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteById(Guid id)
        {
            Book book = await GetEntityById(id);
            unitOfWork.BookRepository.Delete(book);
            await unitOfWork.CompleteAsync();
        }

        public List<BookDTO> GetAll()
        {
            var book = mapper.Map<List<BookDTO>>(unitOfWork.BookRepository.GetAll().Include(x => x.Authors).ToList());
            return book;
        }

        public async Task<BookPreviewDTO> GetById(Guid id)
        {
            Book book = await GetEntityById(id, book => book.Authors);
            var bookDTO = mapper.Map<BookPreviewDTO>(book);
            return bookDTO;
        }

        public async Task Update(BookDTO bookDTO)
        {
            Book book = await GetEntityById(bookDTO.Id);
            mapper.Map(bookDTO, book);
            unitOfWork.BookRepository.Update(book);
            await unitOfWork.CompleteAsync();
        }

        public async Task AddAuthorById(BookAuthorDTO bookAuthorDTO)
        {
            Book book = await GetEntityById(bookAuthorDTO.BookId, book => book.Authors);
            Author author = await authorService.GetEntityById(bookAuthorDTO.AuthorId);
            book.Authors.Add(author);
            unitOfWork.BookRepository.Update(book);
            await unitOfWork.CompleteAsync();
        }

        public async Task RemoveAuthorById(BookAuthorDTO bookAuthorDTO)
        {
            Book book = await GetEntityById(bookAuthorDTO.BookId, book => book.Authors);
            Author author = await authorService.GetEntityById(bookAuthorDTO.AuthorId);
            if (!book.Authors.Remove(author))
            {
                throw new NotFoundException("Author not found");
            }
            unitOfWork.BookRepository.Update(book);
            await unitOfWork.CompleteAsync();
        }

        public async Task<Book> GetEntityById(Guid id, params Expression<Func<Book, object>>[] includes)
        {
            var book = await unitOfWork.BookRepository.GetByIdAsync(id, includes);
            if (book == null)
            {
                throw new NotFoundException("Book not found");
            }
            return book;
        }
    }
}

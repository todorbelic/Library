using AutoMapper;
using BookStoreAPI.DTO.Author;
using BookStoreAPI.DTO.Book;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Service.Interface;
using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.UnitOfWork;
using System.Linq.Expressions;

namespace BookStoreAPI.Service.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task Create(CreateAuthorDTO authorDTO)
        {
            var author = mapper.Map<Author>(authorDTO);
            await unitOfWork.AuthorRepository.Add(author);
            await unitOfWork.CompleteAsync();
        }

        public async Task DeleteById(Guid id)
        {
            Author author = await GetEntityById(id);
            unitOfWork.AuthorRepository.Delete(author);
            await unitOfWork.CompleteAsync();
        }

        public List<AuthorDTO> GetAll()
        {
            var authors = mapper.Map<List<AuthorDTO>>(unitOfWork.AuthorRepository.GetAll().ToList());
            return authors;
        }

        public async Task<AuthorPreviewDTO> GetById(Guid id)
        {
            Author author = await GetEntityById(id, author => author.Books);
            var authorDTO = mapper.Map<AuthorPreviewDTO>(author);
            return authorDTO;
        }

        public async Task Update(AuthorDTO authorDTO)
        {
            Author author = await GetEntityById(authorDTO.Id);
            mapper.Map(authorDTO, author);
            unitOfWork.AuthorRepository.Update(author);
            await unitOfWork.CompleteAsync();
        }

        public async Task<Author> GetEntityById(Guid id, params Expression<Func<Author, object>>[] includes)
        {
            var author = await unitOfWork.AuthorRepository.GetByIdAsync(id, includes);
            if (author == null)
            {
                throw new NotFoundException("Author not found");
            }
            return author;
        }
        
        public async Task<List<BookDTO>> GetAllBooksByAuthorId(Guid id)
        {
            Author author = await GetEntityById(id, author => author.Books);
            return mapper.Map<List<BookDTO>>(author.Books);
        }
    }
}

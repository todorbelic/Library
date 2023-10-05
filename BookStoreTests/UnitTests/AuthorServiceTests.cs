using AutoMapper;
using BookStoreAPI.DTO.Author;
using BookStoreAPI.DTO.Book;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Service.Implementation;
using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.UnitOfWork;
using Moq;
using System.Linq.Expressions;

namespace BookStoreTests.UnitTests
{
    public class AuthorServiceTests
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly IMapper mapper;
        private readonly AuthorService authorService;

        public AuthorServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorDTO>();
                cfg.CreateMap<AuthorDTO, Author>();
                cfg.CreateMap<CreateAuthorDTO, Author>();
                cfg.CreateMap<Author, AuthorPreviewDTO>();
                cfg.CreateMap<Book, BookDTO>();
                cfg.CreateMap<BookDTO, Book>();
                cfg.CreateMap<CreateBookDTO, Book>();
            }));
            authorService = new AuthorService(mockUnitOfWork.Object, mapper);
        }

        [Fact]
        public async Task Create_ShouldCallUnitOfWorkAdd_AndCompleteAsync()
        {
            // Arrange
            var authorDTO = new CreateAuthorDTO { Name = "Test Author" };
            var author = new Author { Name = "Test Author" };
            mockUnitOfWork.Setup(u => u.AuthorRepository.Add(It.IsAny<Author>()))
                .Callback<Author>(a => author = a);
            mockUnitOfWork.Setup(u => u.CompleteAsync()).Verifiable();

            // Act
            await authorService.Create(authorDTO);

            // Assert
            mockUnitOfWork.Verify(u => u.AuthorRepository.Add(author), Times.Once);
            mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteById_ShouldCallUnitOfWorkDelete_AndCompleteAsync()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { Id = authorId };
            mockUnitOfWork.Setup(u => u.AuthorRepository.GetByIdAsync(authorId)).ReturnsAsync(author);
            mockUnitOfWork.Setup(u => u.AuthorRepository.Delete(It.IsAny<Author>())).Verifiable();
            mockUnitOfWork.Setup(u => u.CompleteAsync()).Verifiable();

            // Act
            await authorService.DeleteById(authorId);

            // Assert
            mockUnitOfWork.Verify(u => u.AuthorRepository.GetByIdAsync(authorId), Times.Once);
            mockUnitOfWork.Verify(u => u.AuthorRepository.Delete(author), Times.Once);
            mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturnListOfAuthorDTOs()
        {
            // Arrange
            var authors = new List<Author> { new Author { Id = Guid.NewGuid(), Name = "Test Author 1" }, new Author { Id = Guid.NewGuid(), Name = "Test Author 2" } };
            mockUnitOfWork.Setup(u => u.AuthorRepository.GetAll()).Returns(authors.AsQueryable());

            // Act
            var result = authorService.GetAll();

            // Assert
            Assert.IsType<List<AuthorDTO>>(result);
            Assert.Equal(authors.Count, result.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnAuthorPreviewDTO()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { Id = authorId, Name = "Test Author" };
            mockUnitOfWork.Setup(u => u.AuthorRepository.GetByIdAsync(authorId, It.IsAny<Expression<Func<Author, object>>[]>()))
                .ReturnsAsync(author);

            // Act
            var result = await authorService.GetById(authorId);

            // Assert
            Assert.IsType<AuthorPreviewDTO>(result);
            Assert.Equal(authorId, result.Id);
        }

        [Fact]
        public async Task GetById_ShouldThrowNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            mockUnitOfWork.Setup(u => u.AuthorRepository.GetByIdAsync(authorId, It.IsAny<Expression<Func<Author, object>>[]>()))
                .ReturnsAsync(It.IsAny<Author>);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException>(() => authorService.GetById(authorId));
        }
    }
}

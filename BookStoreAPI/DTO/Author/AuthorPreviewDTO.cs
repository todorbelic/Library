using BookStoreAPI.DTO.Book;

namespace BookStoreAPI.DTO.Author
{
    public class AuthorPreviewDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<BookDTO> Books { get; set; }
    }
}

using BookStoreAPI.DTO.Author;

namespace BookStoreAPI.DTO.Book
{
    public class BookPreviewDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public uint Quantity { get; set; }
        public List<AuthorDTO> Authors { get; set; }
    }
}

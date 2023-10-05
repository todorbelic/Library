using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.DTO.Book
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Title { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public uint Quantity { get; set; }
    }
}
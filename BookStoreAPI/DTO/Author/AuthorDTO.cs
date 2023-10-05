using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.DTO.Author
{
    public class AuthorDTO
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string LastName { get; set; }
    }
}

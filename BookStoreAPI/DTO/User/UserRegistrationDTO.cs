using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStoreAPI.DTO.User
{
    public class UserRegistrationDTO
    {
        [StringLength(30, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string? FirstName { get; set; }

        [StringLength(30, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string? LastName { get; set; }
        [Required]
        [RegularExpression(@"CUSTOMER|ADMINISTRATOR", ErrorMessage = "Role must be CUSTOMER or ADMINISTRATOR")]
        public virtual string Role { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [JsonIgnore]
        public byte[]? Avatar { get; set; } = null;
    }
}

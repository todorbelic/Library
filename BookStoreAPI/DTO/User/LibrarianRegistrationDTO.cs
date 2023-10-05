using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStoreAPI.DTO.User
{
    public class LibrarianRegistrationDTO : UserRegistrationDTO
    {
        [JsonIgnore]
        [RegularExpression(@"LIBRARIAN", ErrorMessage = "Role must be LIBRARIAN")]
        public override string Role { get; set; } = "LIBRARIAN";

    }
}

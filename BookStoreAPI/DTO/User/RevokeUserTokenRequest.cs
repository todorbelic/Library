using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.DTO.User
{
    public class RevokeUserTokenRequest
    {
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string UserEmail { get; set; }
    }
}

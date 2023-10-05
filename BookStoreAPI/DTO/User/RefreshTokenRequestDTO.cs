using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.DTO.User
{
    public class RefreshTokenRequestDTO
    {
        [Required(ErrorMessage = "Access token is required")]
        public string AccessToken { get; set; }
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; }
    }
}

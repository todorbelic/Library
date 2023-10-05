using BookStoreAPI.DTO.User;
using Microsoft.AspNetCore.Identity;

namespace BookStoreAPI.Service.Interface
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDTO userForRegistration);
        Task<UserDTO> GetProfile(string? id);
        Task UpdateUser(string? id, UserDTO userDTO);
        Task UploadAvatar(string id, IFormFile avatar);
        Task<byte[]> GetAvatar(string? id);
        Task RemoveAvatar(string? id);
    }
}

using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.Security;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreAPI.Util
{
    public interface IJwtUtils
    {
        string? ValidateJwtToken(string token);
        Task<RefreshToken> GenerateRefreshToken(string userId, string ipAddress, string userAgent);
        Task<JwtSecurityToken> CreateTokenAsync(User? user);
    }
}

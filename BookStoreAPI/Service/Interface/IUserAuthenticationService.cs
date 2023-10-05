using BookStoreAPI.DTO.User;

namespace BookStoreAPI.Service.Interface
{
    public interface IUserAuthenticationService
    {
        Task<AuthenticateResponseDTO?> ValidateUserLogin(UserLoginDTO loginDto, string ipAddress, string userAgent);
        Task<AuthenticateResponseDTO> RefreshToken(RefreshTokenRequestDTO refreshTokenRequest, string ipAddress, string userAgent);
        Task RevokeAllRefreshTokens(string? ipAddress);
        Task RevokeAllRefreshTokensForUser(RevokeUserTokenRequest revokeRequest, string? ipAddress);
    }
}

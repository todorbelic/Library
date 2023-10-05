namespace BookStoreAPI.DTO.User
{
    public class AuthenticateResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public DateTime AccessTokenExpirationDate { get; set; }

        public AuthenticateResponseDTO(string accessToken, string refreshToken, DateTime accessTokenExpirationDate)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpirationDate = accessTokenExpirationDate;
        }
    }
}

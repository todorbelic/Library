namespace BookStoreAPI.Options
{
    public class JwtOptions
    {
        public static string JwtSection = "JwtConfig";
        public string ValidIssuer { get; init; }
        public string ValidAudience { get; init; }
        public string SecretKey { get; init; }
        public string JwtExpiresInMinutes { get; init; }
        public string RefreshTokenExpiresInDays { get; init; }
    }
}

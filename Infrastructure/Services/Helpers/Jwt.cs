namespace Infrastructure.Services.Helpers
{
    public class Jwt
    {
        public string? Secret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int TokenDurationInMinutes { get; set; }
        public int RefreshTokenDurationInDays { get; set; }
    }
}
namespace API.DTOs
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpirationTime { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpirationTime { get; set; }
    }
}
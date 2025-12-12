namespace Bank_Application.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiry { get; set; }
        public string Role { get; set; } = null!;
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

}

namespace WebApplication1.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = "User";  // default role
    }
}
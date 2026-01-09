namespace WebApplication1.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = default!;//UserNmae
        public string Password { get; set; } = default!; //Pass
        public string Role { get; set; } = "User";  // default role
    }
}
using WebApplication1.Models;
using WebApplication1.DTOs;

public interface IUserService
{
    Task<User?> ValidateUserAsync(string username, string password);

    // New for register
    Task<bool> RegisterAsync(UserRegisterDto dto);

}
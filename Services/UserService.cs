using WebApplication1.Models;
using WebApplication1.Repositories;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.DTOs;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username);
        if (user == null)
            return null;

        // Hash incoming password
        var incomingHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(password)));

        if (incomingHash == user.PasswordHash)
            return user;

        return null;
    }
    public async Task<bool> RegisterAsync(UserRegisterDto dto)
    {
        // 1. Check if username already exists
        var existing = await _repo.GetByUsernameAsync(dto.Username);
        if (existing != null)
            return false;

        // 2. Hash the password
        var passwordHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(dto.Password)));

        // 3. Map DTO -> User entity
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = passwordHash,
            Role = dto.Role
        };

        // 4. Save to DB through repository
        await _repo.AddAsync(user);
        return true;
    }

}
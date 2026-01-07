using WebApplication1.Models;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);

    // For registration
    Task AddAsync(User user);

}
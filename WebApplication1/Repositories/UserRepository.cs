using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

public class UserRepository : IUserRepository
{
    private readonly StudentDbContext _context;

    public UserRepository(StudentDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

}
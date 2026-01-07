using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class StudentDbContext : DbContext
{
    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();

    public DbSet<User> Users { get; set; }
}
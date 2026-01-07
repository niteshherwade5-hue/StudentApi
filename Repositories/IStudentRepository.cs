using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);
        Task SaveChangesAsync();
        Task<List<Student>> GetPagedAsync(int skip, int take, string? search, string? sortBy, string? sortOrder);
        Task<int> GetTotalCountAsync(string? search);
    }
}
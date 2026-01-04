using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _db;

        public StudentRepository(StudentDbContext db)
        {
            _db = db;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _db.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _db.Students.FindAsync(id);
        }

        public async Task AddAsync(Student student)
        {
            await _db.Students.AddAsync(student);
        }

        public void Update(Student student)
        {
            _db.Students.Update(student);
        }

        public void Delete(Student student)
        {
            _db.Students.Remove(student);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
        public async Task<List<Student>> GetPagedAsync(int skip, int take, string? search, string? sortBy, string? sortOrder)
        {
            var query = _db.Students.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    s.SirName.Contains(search) ||
                    s.Email.Contains(search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                bool desc = sortOrder?.ToLower() == "desc";

                query = sortBy.ToLower() switch
                {
                    "name" => desc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                    "age" => desc ? query.OrderByDescending(s => s.Age) : query.OrderBy(s => s.Age),
                    _ => query.OrderBy(s => s.Id)
                };
            }

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(string? search)
        {
            var query = _db.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    s.SirName.Contains(search) ||
                    s.Email.Contains(search));
            }

            return await query.CountAsync();
        }

    }
}
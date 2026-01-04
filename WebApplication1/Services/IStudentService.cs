using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface IStudentService
    {
        Task<List<StudentReadDto>> GetAllAsync();
        Task<StudentReadDto> GetByIdAsync(int id);
        Task<StudentReadDto> CreateAsync(StudentCreateDto dto);
        Task<StudentReadDto> UpdateAsync(int id, StudentCreateDto dto);
        Task DeleteAsync(int id);
        Task<PagedResult<StudentReadDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder);
    }
}
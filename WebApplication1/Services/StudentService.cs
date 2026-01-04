using WebApplication1.DTOs;
using WebApplication1.Mapping;
using WebApplication1.Repositories;
using WebApplication1.Exceptions;
using Microsoft.Extensions.Logging;
using AutoMapper;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;
        private readonly ILogger<StudentService> _logger;
        private readonly IMapper _mapper;


        public StudentService(IStudentRepository repo, ILogger<StudentService> logger, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<StudentReadDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all students");

            var students = await _repo.GetAllAsync();

            _logger.LogInformation("Fetched {Count} students", students.Count);

            return _mapper.Map<List<StudentReadDto>>(students);

        }

        public async Task<StudentReadDto> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching student with ID {Id}", id);

            var student = await _repo.GetByIdAsync(id);

            if (student is null)
            {
                _logger.LogWarning("Student with ID {Id} was not found", id);
                throw new NotFoundException($"Student with ID {id} was not found.");
            }

            _logger.LogInformation("Student with ID {Id} fetched successfully", id);

            return _mapper.Map<StudentReadDto>(student);

        }

        public async Task<StudentReadDto> CreateAsync(StudentCreateDto dto)
        {
            _logger.LogInformation("Creating a new student with Name {Name}", dto.Name);

            var student = _mapper.Map<Student>(dto);

            await _repo.AddAsync(student);
            await _repo.SaveChangesAsync();

            _logger.LogInformation("Student created with ID {Id}", student.Id);

            return _mapper.Map<StudentReadDto>(student);
        }

        public async Task<StudentReadDto> UpdateAsync(int id, StudentCreateDto dto)
        {
            _logger.LogInformation("Updating student with ID {Id}", id);

            var student = await _repo.GetByIdAsync(id);

            if (student is null)
            {
                _logger.LogWarning("Student with ID {Id} not found for update", id);
                throw new NotFoundException($"Student with ID {id} was not found.");
            }

            _mapper.Map(dto, student); // maps onto existing entity
            _repo.Update(student);
            await _repo.SaveChangesAsync();

            _logger.LogInformation("Student with ID {Id} updated successfully", id);

            return _mapper.Map<StudentReadDto>(student);

        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting student with ID {Id}", id);

            var student = await _repo.GetByIdAsync(id);

            if (student is null)
            {
                _logger.LogWarning("Student with ID {Id} not found for deletion", id);
                throw new NotFoundException($"Student with ID {id} was not found.");
            }

            _repo.Delete(student);
            await _repo.SaveChangesAsync();

            _logger.LogInformation("Student with ID {Id} deleted successfully", id);
        }

        public async Task<PagedResult<StudentReadDto>> GetPagedAsync(int page, int pageSize, string? search, string? sortBy, string? sortOrder)
        {
            int skip = (page - 1) * pageSize;

            var students = await _repo.GetPagedAsync(skip, pageSize, search, sortBy, sortOrder);
            var totalCount = await _repo.GetTotalCountAsync(search);

            return new PagedResult<StudentReadDto>
            {
                Items = _mapper.Map<List<StudentReadDto>>(students),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
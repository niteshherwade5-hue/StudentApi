using AutoMapper;
using WebApplication1.Models;
using WebApplication1.DTOs;

namespace WebApplication1.Mapping
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            // Student -> StudentReadDto (for reading data)
            CreateMap<Student, StudentReadDto>();

            // StudentCreateDto -> Student (for creating a new entity)
            CreateMap<StudentCreateDto, Student>();

            // StudentCreateDto -> Student (for updating existing entity)
            // We can use the same map as above and map into an existing object.
            // That will replace UpdateFromDto.
        }
    }
}
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Endpoints
{
    public static class StudentEndpoints
    {
        public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/students");

            group.MapGet("/", GetAllStudents).RequireAuthorization();

            group.MapGet("/{id:int}", GetStudentById).RequireAuthorization();

            group.MapPost("/", CreateStudent).RequireAuthorization();

            group.MapPut("/{id:int}", UpdateStudent)
                 .RequireAuthorization("Admin");

            group.MapDelete("/{id:int}", DeleteStudent)
                 .RequireAuthorization("Admin");

            group.MapGet("/paged", GetPagedStudents).RequireAuthorization();
        }

        private static async Task<IResult> GetAllStudents(IStudentService service)
        {
            var result = await service.GetAllAsync();
            return Results.Ok(result);
        }

        private static async Task<IResult> GetStudentById(int id, IStudentService service)
        {
            var student = await service.GetByIdAsync(id);
            return Results.Ok(student);
        }

        private static async Task<IResult> CreateStudent(
            StudentCreateDto dto,
            IStudentService service,
            IValidator<StudentCreateDto> validator)
        {
            var validation = await validator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage });

                return Results.BadRequest(errors);
            }

            var created = await service.CreateAsync(dto);
            return Results.Created($"/students/{created.Id}", created);
        }

        private static async Task<IResult> UpdateStudent(
            int id,
            StudentCreateDto dto,
            IStudentService service,
            IValidator<StudentCreateDto> validator)
        {
            var validation = await validator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage });

                return Results.BadRequest(errors);
            }

            var updated = await service.UpdateAsync(id, dto);
            return Results.Ok(updated);
        }

        private static async Task<IResult> DeleteStudent(int id, IStudentService service)
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        }

        private static async Task<IResult> GetPagedStudents(
            int page,
            int pageSize,
            string? search,
            string? sortBy,
            string? order,
            IStudentService service)
        {
            var result = await service.GetPagedAsync(page, pageSize, search, sortBy, order);
            return Results.Ok(result);
        }
    }
}
using FluentValidation;
using WebApplication1.DTOs;
using WebApplication1.Services;
using WebApplication1.Helpers;

namespace WebApplication1.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/users").WithTags("Auth (User End-Points)");

            group.MapPost("/register", RegisterUser);
            group.MapPost("/login", LoginUser);
        }

        private static async Task<IResult> RegisterUser(
            UserRegisterDto dto,
            IUserService userService,
            IValidator<UserRegisterDto> validator)
        {
            var validation = await validator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errors = validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage });

                return Results.BadRequest(errors);
            }

            var success = await userService.RegisterAsync(dto);

            if (!success)
                return Results.BadRequest("Username already exists");

            return Results.Ok("User registered successfully");
        }

        private static async Task<IResult> LoginUser(
            UserLoginDto dto,
            IUserService userService,
            IConfiguration config)
        {
            var user = await userService.ValidateUserAsync(dto.Username, dto.Password);

            if (user == null)
                return Results.Unauthorized();

            var token = JwtTokenGenerator.GenerateToken(user.Username, user.Role, config);
            return Results.Ok(new { token });
        }
    }
}
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.DTOs;
using WebApplication1.Mapping;
using WebApplication1.Repositories;
using WebApplication1.Services;
using WebApplication1.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using WebApplication1.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Helpers;
using System.Security.Cryptography;
using WebApplication1.Endpoints;



var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<StudentCreateDtoValidator>();

builder.Services.AddAutoMapper(typeof(StudentProfile));

// Register DbContext
builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StudentDbContext>();

    // Seed admin user if not exists
    if (!context.Users.Any())
    {
        var passwordHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes("123")));

        context.Users.Add(new User
        {
            Username = "admin",
            PasswordHash = passwordHash,
            Role = "Admin"
        });

        context.SaveChanges();
    }

    // Seed additional user if not exists
    if (!context.Users.Any(u => u.Username == "Nitesh"))
    {
        var johnPasswordHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes("12345")));

        context.Users.Add(new User
        {
            Username = "Nitesh",
            PasswordHash = johnPasswordHash,
            Role = "User"
        });

        context.SaveChanges();
    }

}


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapStudentEndpoints();
app.MapUserEndpoints();

app.Run();







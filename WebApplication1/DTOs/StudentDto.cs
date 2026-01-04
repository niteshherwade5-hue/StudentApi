namespace WebApplication1.DTOs;

public record StudentCreateDto(
    string Name,
    string SirName,
    int Age,
    string Email
);

public record StudentReadDto(
    int Id,
    string Name,
    string SirName,
    int Age
    //string Email
);
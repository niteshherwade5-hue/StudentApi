using FluentValidation;
using WebApplication1.DTOs;

namespace WebApplication1.Validation
{
    public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
    {
        public StudentCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters");

            RuleFor(x => x.SirName)
                .NotEmpty().WithMessage("SirName is required")
                .MaximumLength(50).WithMessage("SirName cannot be longer than 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.Age)
                .InclusiveBetween(1, 120).WithMessage("Age must be between 1 and 120");
        }
    }
}
using FluentValidation;
using WebApplication1.DTOs;

namespace WebApplication1.Validation
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(3).WithMessage("Password must be at least 3 characters");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required");
        }
    }
}
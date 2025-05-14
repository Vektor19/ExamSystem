using FluentValidation;

namespace ExamSystem.Application.DTOs.User
{
    public class CreateUserByAdminDtoValidator : AbstractValidator<CreateUserByAdminDto>
    {
        public CreateUserByAdminDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .Length(2, 50)
                .WithMessage("First name must be between 2 and 50 characters.");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .Length(2, 50)
                .WithMessage("Last name must be between 2 and 50 characters.");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.");
            RuleFor(x => x.Roles)
                .NotEmpty()
                .WithMessage("At least one role is required.")
                .Must(roles => roles.Count > 0)
                .WithMessage("At least one role must be specified.");
        }
    }
}

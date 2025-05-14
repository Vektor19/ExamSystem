using ExamSystem.Application.DTOs.Violation;
using ExamSystem.Core.Enums;
using FluentValidation;

namespace ExamSystem.Application.DTOs.User
{
    public class CreateViolationDtoValidator : AbstractValidator<CreateViolationDto>
    {
        public CreateViolationDtoValidator()
        {
            RuleFor(x => x.ExamUserId)
                .NotEmpty()
                .WithMessage("ExamUserId is required.");
            RuleFor(x => x.ViolationType)
                .NotEmpty()
                .WithMessage("ViolationType is required.")
                .Must(type => Enum.TryParse(type, true, out ViolationType _))
                .WithMessage("ViolationType must be a valid ViolationType.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");
        }
    }
}

using ExamSystem.Application.DTOs.Exam;
using FluentValidation;

namespace ExamSystem.Application.DTOs.User
{
    public class AddParticipantByEmailDtoValidator : AbstractValidator<AddParticipantByEmailDto>
    {
        public AddParticipantByEmailDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
        }
    }
}

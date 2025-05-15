using ExamSystem.Application.DTOs.Exam;
using FluentValidation;

namespace ExamSystem.Application.DTOs.User
{
    public class RemoveParticipantByEmailDtoValidator : AbstractValidator<RemoveParticipantByEmailDto>
    {
        public RemoveParticipantByEmailDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
        }
    }
}

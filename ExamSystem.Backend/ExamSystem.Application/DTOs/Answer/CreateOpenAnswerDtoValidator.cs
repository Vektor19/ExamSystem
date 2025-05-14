using ExamSystem.Application.DTOs.Exam;
using FluentValidation;

namespace ExamSystem.Application.DTOs.User
{
    public class CreateOpenAnswerDtoValidator : AbstractValidator<CreateOpenAnswerDto>
    {
        public CreateOpenAnswerDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
            RuleFor(x => x.ExamId)
                .NotEmpty()
                .WithMessage("ExamId is required.");
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required.");
            RuleFor(x => x.AnswerText)
                .NotEmpty()
                .WithMessage("AnswerText is required.");
        }
    }
}

using ExamSystem.Application.DTOs.Question;
using ExamSystem.Core.Enums;
using FluentValidation;

namespace ExamSystem.Application.DTOs.User
{
    public class QuestionCreateDtoValidator : AbstractValidator<QuestionCreateDto>
    {
        public QuestionCreateDtoValidator()
        {
            RuleFor(x => x.ExamId)
                .NotEmpty()
                .WithMessage("ExamId is required.");
            RuleFor(x => x.QuestionText)
                .NotEmpty()
                .WithMessage("QuestionText is required.");
            RuleFor(x => x.Type)
                .Must(type => Enum.TryParse<QuestionType>(type, true, out _))
                .WithMessage("Type must be a valid QuestionType: MultiChoice or Text.");
            RuleFor(x => x.MaxPoints)
                .GreaterThan(0)
                .WithMessage("MaxPoints must be greater than 0.");
        }
    }
}

using ExamSystem.Application.DTOs.Exam;
using ExamSystem.Application.DTOs.Question;
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
                .IsInEnum()
                .WithMessage("Type must be either MultiChoice or Text.");
            RuleFor(x => x.MaxPoints)
                .GreaterThan(0)
                .WithMessage("MaxPoints must be greater than 0.");
        }
    }
}

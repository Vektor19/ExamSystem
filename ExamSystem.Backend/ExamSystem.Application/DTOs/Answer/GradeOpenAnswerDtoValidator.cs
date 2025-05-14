using ExamSystem.Application.DTOs.Exam;
using FluentValidation;

namespace ExamSystem.Application.DTOs.User
{
    public class GradeOpenAnswerDtoValidator : AbstractValidator<GradeOpenAnswerDto>
    {
        public GradeOpenAnswerDtoValidator()
        {
            RuleFor(x => x.ExamUserId)
                .NotEmpty()
                .WithMessage("ExamUserId is required.");
            RuleFor(x => x.AnswerId)
                .NotEmpty()
                .WithMessage("AnswerId is required.");
            RuleFor(x => x.Grade)
                .NotEmpty()
                .WithMessage("Grade is required.");
        }
    }
}

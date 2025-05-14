using FluentValidation;

namespace ExamSystem.Application.DTOs.User
{
    public class ExamCreateDtoValidator : AbstractValidator<ExamCreateDto>
    {
        public ExamCreateDtoValidator()
        {
            RuleFor(x => x.CreatedByUserId)
                .NotEmpty()
                .WithMessage("CreatedByUserId is required.");
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("StartDate is required.")
                .Must(startDate => startDate > DateTime.UtcNow)
                .WithMessage("StartDate must be in the future.");
            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("EndDate is required.")
                .Must((dto, endDate) => endDate > dto.StartDate)
                .WithMessage("EndDate must be after StartDate.");
        }
    }
}

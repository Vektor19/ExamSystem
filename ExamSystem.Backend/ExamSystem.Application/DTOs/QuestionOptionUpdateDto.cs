namespace ExamSystem.Application.DTOs
{
    public class QuestionOptionUpdateDto
    {
        public string Label { get; set; } = string.Empty;
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;

    }
}

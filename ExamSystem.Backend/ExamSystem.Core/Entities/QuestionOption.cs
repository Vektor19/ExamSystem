namespace ExamSystem.Core.Entities
{
    public class QuestionOption
    {
        public Guid QuestionOptionId { get; set; }
        public Guid QuestionId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
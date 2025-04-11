using ExamSystem.Core.Enums;

namespace ExamSystem.Core.Entities
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public Guid ExamId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public List<QuestionOption> QuestionOptions { get; set; } = [];
        public Exam Exam { get; set; } = null!;
    }
}

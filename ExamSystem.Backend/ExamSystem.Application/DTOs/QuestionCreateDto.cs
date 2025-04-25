namespace ExamSystem.Application.DTOs
{
    public class QuestionCreateDto
    {
        public Guid ExamId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<QuestionOptionCreateDto> Options { get; set; } = [];
    }
}

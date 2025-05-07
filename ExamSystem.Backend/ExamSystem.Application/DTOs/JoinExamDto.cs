namespace ExamSystem.Application.DTOs
{
    public class JoinExamDto
    {
        public Guid UserId { get; set; }
        public string JoinCode { get; set; } = string.Empty;
    }
}

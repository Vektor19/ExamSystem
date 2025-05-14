namespace ExamSystem.Application.DTOs
{
    public class GradeOpenAnswerDto
    {
        public Guid ExamUserId { get; set; }
        public Guid AnswerId { get; set; }
        public double Grade { get; set; }
    }
}

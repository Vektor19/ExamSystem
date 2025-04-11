namespace ExamSystem.Core.Entities
{
    public class ExamUser
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; }
        public bool CompleteStatus { get; set; }
    }
}

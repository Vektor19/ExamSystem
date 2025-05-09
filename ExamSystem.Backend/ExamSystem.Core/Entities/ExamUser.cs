namespace ExamSystem.Core.Entities
{
    public class ExamUser
    {
        public Guid ExamUserId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
        public bool CompleteStatus { get; set; }
        public bool IsBlocked { get; set; }
        public int Grade { get; set; }
        public List<Violation> Violations { get; set; } = [];
    }
}

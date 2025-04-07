using ExamSystem.Core.Enums;

namespace ExamSystem.Core.Entities
{
    public class Exam
    {
        public Guid ExamId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ExamStatus Status { get; set; }
    }
}

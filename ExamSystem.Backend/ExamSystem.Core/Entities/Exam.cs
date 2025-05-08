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
        public string JoinCode { get; set; } = string.Empty;
        public User UserCreatedBy { get; set; } = null!;
        public List<ExamUser> ExamUsers { get; set; } = [];
        public List<Question> Questions { get; set; } = [];
        public List<Answer> Answers { get; set; } = [];
        public ExamStatus Status
        {
            get
            {
                if (DateTime.UtcNow < StartDate)
                    return ExamStatus.NotStarted;
                if (DateTime.UtcNow > EndDate)
                    return ExamStatus.Finished;
                return ExamStatus.Started;
            }
        }
    }
}
namespace ExamSystem.Core.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<Role> Roles { get; set; } = [];
        public List<ExamUser> ExamUsers { get; set; } = [];
        public List<Exam> CreatedExams { get; set; } = [];
        public List<Answer> Answers { get; set; } = [];
    }
}

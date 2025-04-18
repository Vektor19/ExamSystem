namespace ExamSystem.Core.Common
{
    public static class SystemRoles
    {
        public const string Admin = "Admin";
        public const string Student = "Student";
        public const string Examinator = "Examinator";
        public static readonly string[] All = [Admin, Student, Examinator];
    }
}

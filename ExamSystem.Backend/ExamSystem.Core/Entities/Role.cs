namespace ExamSystem.Core.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSystem { get; set; }
        public List<UserRole> UserRoles { get; set; } = [];
    }
}

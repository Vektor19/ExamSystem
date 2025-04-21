using ExamSystem.Core.Enums;

namespace ExamSystem.Application.DTOs
{
    public class ExamUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ExamStatus Status { get; set; }
    }
}

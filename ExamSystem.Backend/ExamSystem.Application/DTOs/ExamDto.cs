using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class ExamDto
    {
        public Guid ExamId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string JoinCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public UserDto CreatedBy { get; set; } = null!;
        public int QuestionCount { get; set; }
        public int ParticipantCount { get; set; }
    }
}

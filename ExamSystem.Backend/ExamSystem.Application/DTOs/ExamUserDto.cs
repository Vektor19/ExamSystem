using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class ExamUserDto
    {
        public Guid ExamUserId { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool CompleteStatus { get; set; }
        public bool IsBlocked { get; set; }
        public int Grade { get; set; }
        public List<ViolationDto> Violations { get; set; } = [];
    }
}

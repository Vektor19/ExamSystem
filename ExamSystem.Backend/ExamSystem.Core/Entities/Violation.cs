using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamSystem.Core.Enums;

namespace ExamSystem.Core.Entities
{
    public class Violation
    {
        public Guid ViolationId { get; set; }
        public Guid ExamUserId { get; set; }
        public ViolationType ViolationType { get; set; }
        public string Description { get; set; } = string.Empty;
        public ExamUser ExamUser { get; set; } = null!;
    }
}

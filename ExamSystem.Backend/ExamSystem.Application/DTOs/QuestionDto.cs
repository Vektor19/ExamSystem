using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    internal class QuestionDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<QuestionOptionDto> Options { get; set; } = [];
    }
}

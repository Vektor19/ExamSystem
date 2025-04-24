using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class QuestionCreateDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public Guid ExamId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<QuestionOptionCreateDto> Options { get; set; } = [];
    }
}

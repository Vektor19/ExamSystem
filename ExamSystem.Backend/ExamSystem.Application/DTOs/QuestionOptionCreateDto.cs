using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class QuestionOptionCreateDto
    {
        public Guid QuestionId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string OptionText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}

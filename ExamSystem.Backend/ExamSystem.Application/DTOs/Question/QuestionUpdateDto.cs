using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamSystem.Application.DTOs.QuestionOption;

namespace ExamSystem.Application.DTOs.Question
{
    public class QuestionUpdateDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public double MaxPoints { get; set; }
        public List<QuestionOptionUpdateDto> Options { get; set; } = [];
    }
}

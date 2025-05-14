using System;
using System.Collections.Generic;
using System.Linq;
namespace ExamSystem.Application.DTOs.QuestionOption
{
    public class QuestionOptionDto
    {
        public Guid QuestionOptionId { get; set; }
        public Guid QuestionId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string OptionText { get; set; } = string.Empty;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class CreateOpenAnswerDto : CreateAnswerDto
    {
        public string AnswerText { get; set; } = string.Empty;
    }
}

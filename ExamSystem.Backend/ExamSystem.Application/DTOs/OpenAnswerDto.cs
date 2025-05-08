using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class OpenAnswerDto: AnswerDto
    {
        public string AnswerText { get; set; } = string.Empty;
    }
}

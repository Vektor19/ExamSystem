using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class CreateOptionAnswerDto : CreateAnswerDto
    {
        public Guid QuestionOptionId { get; set; }
    }
}

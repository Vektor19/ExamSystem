using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class AnswerDto
    {
        public Guid AnswerId { get; set; }
        public Guid UserId { get; set; }
        public Guid QuestionOptionId { get; set; }
    }
}

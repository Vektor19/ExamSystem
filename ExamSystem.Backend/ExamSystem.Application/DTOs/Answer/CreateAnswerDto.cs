using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Application.DTOs
{
    public class CreateAnswerDto
    {
        public Guid UserId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid ExamId { get; set; }
    }
}

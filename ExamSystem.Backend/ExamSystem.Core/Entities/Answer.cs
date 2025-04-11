using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem.Core.Entities
{
    public class Answer
    {
        public Guid AnswerId { get; set; }
        public Guid UserId { get; set; }
        public Guid QuestionOptionId { get; set; }
        public User User { get; set; } = null!;
        public QuestionOption QuestionOption { get; set; } = null!;
    }
}

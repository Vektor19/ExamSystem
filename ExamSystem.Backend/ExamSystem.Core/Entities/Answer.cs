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
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? QuestionOptionId { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsGraded { get; set; }
        public User User { get; set; } = null!;
        public Question Question { get; set; } = null!;
        public QuestionOption? QuestionOption { get; set; }
        public Exam Exam { get; set; } = null!;
    }
}

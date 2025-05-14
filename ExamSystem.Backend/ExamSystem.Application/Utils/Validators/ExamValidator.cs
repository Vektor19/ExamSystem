using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;

namespace ExamSystem.Application.Utils.Validators
{
    public static class ExamValidator
    {
        public static bool IsModifyAllowed(Exam exam)
        {
            return exam.Status == ExamStatus.NotStarted;
        }
    }
}

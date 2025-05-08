using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamSystem.Core.Common;
using ExamSystem.Core.Entities;
using ExamSystem.Core.Enums;

namespace ExamSystem.Application.Utils.Validators
{
    public static class ExamValidator
    {
        public static OperationResult IsModifyAllowed(Exam exam)
        {
            return exam.Status == ExamStatus.NotStarted
                ? OperationResult.Ok()
                : OperationResult.Fail("Operation allowed only when exam status is NotStarted.");
        }
        public static OperationResult ValidateDates(Exam exam)
        {
            if (exam.StartDate < DateTime.UtcNow)
                return OperationResult.Fail("Start date must be in the future.");
            if (exam.EndDate <= exam.StartDate)
                return OperationResult.Fail("End date must be after start date.");
            return OperationResult.Ok();
        }
    }
}

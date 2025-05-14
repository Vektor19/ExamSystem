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
        public static ServiceOperationResult IsModifyAllowed(Exam exam)
        {
            return exam.Status == ExamStatus.NotStarted
                ? ServiceOperationResult.Ok()
                : ServiceOperationResult.Fail("Operation allowed only when exam status is NotStarted.");
        }
        public static ServiceOperationResult ValidateDates(Exam exam)
        {
            if (exam.StartDate < DateTime.UtcNow)
                return ServiceOperationResult.Fail("Start date must be in the future.");
            if (exam.EndDate <= exam.StartDate)
                return ServiceOperationResult.Fail("End date must be after start date.");
            return ServiceOperationResult.Ok();
        }
    }
}

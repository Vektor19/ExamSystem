using ExamSystem.Application.Common.Models;
using ExamSystem.Application.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ExamSystem.API.Extensions
{

    public static class ControllerExtensions
    {
        public static IActionResult ToActionResult(this ServiceOperationResult result)
        {
            if (result.Success)
                return new OkObjectResult(result);

            return result.ErrorType switch
            {
                ServiceOperationErrorType.NotFound => new NotFoundObjectResult(result.ErrorMessage),
                ServiceOperationErrorType.Validation => new BadRequestObjectResult(result.ErrorMessage),
                ServiceOperationErrorType.Conflict => new ConflictObjectResult(result.ErrorMessage),
                ServiceOperationErrorType.Forbidden => new ForbidResult(),
                ServiceOperationErrorType.Internal => new ObjectResult(result.ErrorMessage) { StatusCode = 500 },
                ServiceOperationErrorType.Unauthorized => new UnauthorizedObjectResult(result.ErrorMessage),
                _ => new ObjectResult(result.ErrorMessage) { StatusCode = 500 }
            };
        }

        public static IActionResult ToActionResult<T>(this ServiceOperationResult<T> result)
        {
            if (result.Success)
                return new OkObjectResult(result.Data);

            return result.ErrorType switch
            {
                ServiceOperationErrorType.NotFound => new NotFoundObjectResult(result.ErrorMessage),
                ServiceOperationErrorType.Validation => new BadRequestObjectResult(result.ErrorMessage),
                ServiceOperationErrorType.Conflict => new ConflictObjectResult(result.ErrorMessage),
                ServiceOperationErrorType.Forbidden => new ForbidResult(),
                ServiceOperationErrorType.Internal => new ObjectResult(result.ErrorMessage) { StatusCode = 500 },
                ServiceOperationErrorType.Unauthorized => new UnauthorizedObjectResult(result.ErrorMessage),
                _ => new ObjectResult(result.ErrorMessage) { StatusCode = 500 }
            };
        }
    }

}

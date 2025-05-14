namespace ExamSystem.Application.Common.Enums;

public enum ServiceOperationErrorType
{
    Unknown,
    NotFound,
    Validation,
    Conflict,
    Forbidden,
    Internal,
    Unauthorized,
    BadRequest,
}
using ExamSystem.Application.Common.Enums;

namespace ExamSystem.Application.Common.Models;
public class ServiceOperationResult
{
    public bool Success { get; private set; }
    public string? ErrorMessage { get; private set; }
    public ServiceOperationErrorType? ErrorType { get; private set; }
    private ServiceOperationResult(bool success, string? errorMessage = null, ServiceOperationErrorType? errorType = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
        ErrorType = errorType;
    }
    public static ServiceOperationResult Ok() => new(true);
    public static ServiceOperationResult Fail(string error, ServiceOperationErrorType type) => new(false, error, type);
}
public class ServiceOperationResult<T>
{
    public bool Success { get; private set; }
    public string? ErrorMessage { get; private set; }
    public ServiceOperationErrorType? ErrorType { get; private set; }
    public T? Data { get; private set; }
    private ServiceOperationResult(bool success, T? data = default, string? errorMessage = null, ServiceOperationErrorType? errorType = null)
    {
        Success = success;
        Data = data;
        ErrorMessage = errorMessage;
        ErrorType = errorType;
    }
    public static ServiceOperationResult<T> Ok(T data) => new(true, data);
    public static ServiceOperationResult<T> Fail(string error, ServiceOperationErrorType type) => new(false, default, error, type);
}

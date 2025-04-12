namespace ExamSystem.Core.Common
{
    public class OperationResult
    {
        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }
        private OperationResult(bool success, string? errorMessage = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
        public static OperationResult Ok() => new(true);
        public static OperationResult Fail(string error) => new(false, error);
    }
    public class OperationResult<T>
    {
        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }
        public T? Data { get; private set; }
        private OperationResult(bool success, T? data = default, string? errorMessage = null)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
        }
        public static OperationResult<T> Ok(T data) => new(true, data);
        public static OperationResult<T> Fail(string error) => new(false, default, error);
    }
}

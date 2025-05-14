namespace ExamSystem.Core.Common
{
    public class RepositoryOperationResult
    {
        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }
        private RepositoryOperationResult(bool success, string? errorMessage = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
        public static RepositoryOperationResult Ok() => new(true);
        public static RepositoryOperationResult Fail(string error) => new(false, error);
    }
    public class RepositoryOperationResult<T>
    {
        public bool Success { get; private set; }
        public string? ErrorMessage { get; private set; }
        public T? Data { get; private set; }
        private RepositoryOperationResult(bool success, T? data = default, string? errorMessage = null)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
        }
        public static RepositoryOperationResult<T> Ok(T data) => new(true, data);
        public static RepositoryOperationResult<T> Fail(string error) => new(false, default, error);
    }
}

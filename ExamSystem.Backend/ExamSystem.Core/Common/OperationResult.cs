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
}

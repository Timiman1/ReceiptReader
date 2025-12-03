namespace ReceiptReader.Domain.Shared
{
    // <summary>
    /// Used for operations that do not return a value (e.g., delete, update)
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public bool IsFailure => !IsSuccess;
        public FailureType FailureType { get; }

        protected Result(
            bool isSuccess, 
            string errorMessage, 
            FailureType failureType)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true, string.Empty, FailureType.None);

        public static Result Fail(string message, FailureType type) => new Result(false, message, type);
    }
}

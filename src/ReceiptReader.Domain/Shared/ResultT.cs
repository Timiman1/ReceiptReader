namespace ReceiptReader.Domain.Shared
{
    /// <summary>
    /// Used for operations that return a value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultT<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("Cannot access Value on a failed Result.");
                }
                return _value;
            }
        }

        // Success constructor
        private ResultT(T value) : base(true, string.Empty, FailureType.None)
        {
            _value = value;
        }

        // Failure constructor
        private ResultT(string errorMessage, FailureType failureType) : base(false, errorMessage, failureType)
        {
            _value = default!;
        }

        public static ResultT<T> Success(T value) => new ResultT<T>(value);

        // Allows returning a generic failure message
        public static new ResultT<T> Fail(string message, FailureType type) => new ResultT<T>(message, type);

    }
}

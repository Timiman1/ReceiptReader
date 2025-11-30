namespace ReceiptReader.Application.Exceptions
{
    /// <summary>
    /// Thrown when a receipt analyzer operation fails.
    /// </summary>
    public class ReceiptAnalyzerException : Exception
    {
        public ReceiptAnalyzerException()
        {
        }

        public ReceiptAnalyzerException(string message)
            : base(message)
        {
        }

        public ReceiptAnalyzerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

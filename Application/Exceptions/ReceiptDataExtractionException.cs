namespace ReceiptReader.Application.Exceptions
{
    /// <summary>
    /// Thrown when a receipt data extraction operation fails.
    /// </summary>
    public class ReceiptDataExtractionException : Exception
    {
        public ReceiptDataExtractionException()
        {
        }

        public ReceiptDataExtractionException(string message)
            : base(message)
        {
        }

        public ReceiptDataExtractionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

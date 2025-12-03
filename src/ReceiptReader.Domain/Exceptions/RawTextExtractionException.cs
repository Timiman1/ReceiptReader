namespace ReceiptReader.Application.Exceptions
{
    /// <summary>
    /// Thrown when a raw text extraction operation fails.
    /// </summary>
    public class RawTextExtractionException : Exception
    {
        public RawTextExtractionException()
        {
        }

        public RawTextExtractionException(string message)
            : base(message)
        {
        }

        public RawTextExtractionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

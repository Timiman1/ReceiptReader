using ReceiptReader.Domain;

namespace ReceiptReader.Application.Services
{
    /// <summary>
    /// Service for handling receipt upload and processing operations.
    /// Orchestrates validation, storage, analysis, and persistence.
    /// </summary>
    public interface IReceiptService
    {
        /// <summary>
        /// Processes an uploaded receipt file.
        /// </summary>
        Task<ReceiptServiceResult> ProcessReceiptAsync(Stream fileStream, string fileName, string contentType, long fileLength);
    }

    /// <summary>
    /// Base result type for receipt service operations.
    /// Use pattern matching or IsSuccess to determine the actual result type.
    /// </summary>
    public abstract class ReceiptServiceResult
    {
        public abstract bool IsSuccess { get; }
    }

    /// <summary>
    /// Represents a successful receipt processing result.
    /// </summary>
    public class ReceiptServiceSuccess : ReceiptServiceResult
    {
        public override bool IsSuccess => true;
        public ReceiptInfo Receipt { get; }

        public ReceiptServiceSuccess(ReceiptInfo receipt)
        {
            Receipt = receipt ?? throw new ArgumentNullException(nameof(receipt));
        }
    }

    /// <summary>
    /// Represents a failed receipt processing result.
    /// </summary>
    public class ReceiptServiceFailure : ReceiptServiceResult
    {
        public override bool IsSuccess => false;
        public string ErrorMessage { get; }

        public ReceiptServiceFailure(string errorMessage)
        {
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }
    }
}

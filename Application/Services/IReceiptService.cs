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

    public class ReceiptServiceResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public Domain.ReceiptInfo? Receipt { get; set; }
    }
}

namespace ReceiptReader.Application.Services
{
    /// <summary>
    /// Service for handling file retrieval operations.
    /// </summary>
    public interface IFileRetrievalService
    {
        /// <summary>
        /// Retrieves a file by its ID.
        /// </summary>
        Task<FileRetrievalResult> GetFileAsync(Guid fileId);
    }

    public class FileRetrievalResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public Stream? FileStream { get; set; }
        public string? ContentType { get; set; }
    }
}

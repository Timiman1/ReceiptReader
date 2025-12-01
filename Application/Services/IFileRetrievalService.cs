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

    /// <summary>
    /// Base result type for file retrieval operations.
    /// </summary>
    public abstract class FileRetrievalResult
    {
        public abstract bool IsSuccess { get; }
    }

    /// <summary>
    /// Represents a successful file retrieval result.
    /// </summary>
    public class FileRetrievalSuccess : FileRetrievalResult
    {
        public override bool IsSuccess => true;
        public Stream FileStream { get; }
        public string ContentType { get; }

        public FileRetrievalSuccess(Stream fileStream, string contentType)
        {
            FileStream = fileStream ?? throw new ArgumentNullException(nameof(fileStream));
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
        }
    }

    /// <summary>
    /// Represents a failed file retrieval result.
    /// </summary>
    public class FileRetrievalFailure : FileRetrievalResult
    {
        public override bool IsSuccess => false;
        public string ErrorMessage { get; }

        public FileRetrievalFailure(string errorMessage)
        {
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }
    }
}

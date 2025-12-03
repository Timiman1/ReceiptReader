namespace ReceiptReader.Application.Validation
{
    /// <summary>
    /// Defines the contract for validating file types, checking if a file's extension or MIME type is allowed by the system.
    /// </summary>
    public interface IFileValidator
    {
        /// <summary>
        /// Summary in progress
        /// </summary>
        bool HasAllowedExtension(string fileName);

        /// <summary>
        /// Summary in progress
        /// </summary>
        bool IsAllowedMimeType(string contentType);
    }
}

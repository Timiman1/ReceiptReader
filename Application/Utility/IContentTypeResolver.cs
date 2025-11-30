namespace ReceiptReader.Application.Utility
{
    /// <summary>
    /// Represents the service used for resolving content types based on file extensions.
    /// </summary>
    public interface IContentTypeResolver
    {
        // Method summary is skipped, as the interface summary defines the contract.
        string GetContentType(string extension);
    }
}

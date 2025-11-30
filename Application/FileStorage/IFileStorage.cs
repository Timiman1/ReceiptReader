namespace ReceiptReader.Application.FileStorage
{
    /// <summary>
    /// Represents the service responsible for persisting, fetching and handling files either locally or up in the cloud.
    /// </summary>
    public interface IFileStorage
    {
        /// <summary>
        /// Summary in progress
        /// </summary>
        Task SaveAsync(
            Stream stream,
            string fileName,
            string contentType,
            Guid fileId);

        /// <summary>
        /// Summary in progress
        /// </summary>
        Task<Stream> OpenReadAsync(Guid fileId);

        /// <summary>
        /// Summary in progress
        /// </summary>
        string? TryGetExtension(Guid fileId);
    }
}

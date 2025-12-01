using ReceiptReader.Application.FileStorage;
using ReceiptReader.Application.Services;
using ReceiptReader.Application.Utility;

namespace ReceiptReader.Infrastructure.Services
{
    /// <summary>
    /// Implementation of file retrieval service.
    /// Handles fetching files from storage with proper content type resolution.
    /// </summary>
    public class FileRetrievalService : IFileRetrievalService
    {
        private readonly IFileStorage _fileStorage;
        private readonly IContentTypeResolver _contentTypeResolver;

        public FileRetrievalService(
            IFileStorage fileStorage,
            IContentTypeResolver contentTypeResolver)
        {
            _fileStorage = fileStorage;
            _contentTypeResolver = contentTypeResolver;
        }

        public async Task<FileRetrievalResult> GetFileAsync(Guid fileId)
        {
            try
            {
                var extension = _fileStorage.TryGetExtension(fileId);

                if (extension == null)
                {
                    return new FileRetrievalResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "File not found."
                    };
                }

                var stream = await _fileStorage.OpenReadAsync(fileId);
                var contentType = _contentTypeResolver.GetContentType(extension)
                    ?? "application/octet-stream";

                return new FileRetrievalResult
                {
                    IsSuccess = true,
                    FileStream = stream,
                    ContentType = contentType
                };
            }
            catch (FileNotFoundException)
            {
                return new FileRetrievalResult
                {
                    IsSuccess = false,
                    ErrorMessage = "File not found."
                };
            }
        }
    }
}

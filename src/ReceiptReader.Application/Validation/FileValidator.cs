using Microsoft.Extensions.Options;
using ReceiptReader.Application.Validation;
using ReceiptReader.Application.Configurations;

namespace ReceiptReader.Infrastructure.Validation
{
    public class FileValidator : IFileValidator
    {
        private readonly FileSettings _fileSettings;

        public FileValidator(IOptions<FileSettings> fileSettingsOptions)
        {
            _fileSettings = fileSettingsOptions.Value;
        }

        public bool HasAllowedExtension(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext))
            {
                return false;
            }
            return _fileSettings.AllowedExtensions
                                .Contains(ext.ToLowerInvariant());
        }

        public bool IsAllowedMimeType(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return false; 
            }
            return _fileSettings.AllowedMimeTypes
                                .Contains(contentType.ToLowerInvariant());
        }
    }
}

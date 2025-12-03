using ReceiptReader.Application.Utility;

namespace ReceiptReader.Application.Utility
{
    public class ContentTypeResolver : IContentTypeResolver
    {
        public string GetContentType(string extension)
        {
            return extension.ToLowerInvariant() switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".tif" or ".tiff" => "image/tiff",
                _ => "application/octet-stream"
            };
        }
    }
}

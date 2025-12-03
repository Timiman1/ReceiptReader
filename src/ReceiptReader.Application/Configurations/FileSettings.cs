namespace ReceiptReader.Application.Configurations
{
    public class FileSettings
    {
        public const string SectionName = "FileSettings";

        public long MaxFileSizeInBytes { get; set; }
        public string[] AllowedExtensions { get; set; } = [];
        public string[] AllowedMimeTypes { get; set; } = [];
    }
}

namespace ReceiptReader.Infrastructure.Configurations
{
    public class TesseractOptions
    {
        public const string SectionName = "RawTextExtractionProviders:Tesseract";

        public string Language { get; set; } = string.Empty;
    }
}

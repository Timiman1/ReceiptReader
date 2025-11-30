namespace ReceiptReader.Infrastructure.Configurations
{
    public class ReceiptDataExtractionOptions
    {
        public const string SectionName = "ReceiptDataExtraction";

        public double MinRequiredExtractionConfidence { get; set; }
        public string DefaultCurrencyIfNotFound { get; set; } = string.Empty;
    }
}

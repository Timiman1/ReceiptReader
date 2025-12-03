namespace ReceiptReader.Infrastructure.Configurations
{
    public class AzureAIDocumentIntelligenceOptions
    {
        public const string SectionName = "RawTextExtractionProviders:AzureAIDocumentIntelligence";

        public string Endpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ModelId { get; set; } = string.Empty;
    }
}

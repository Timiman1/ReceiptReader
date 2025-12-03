
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Extensions.Options;
using ReceiptReader.Application.Exceptions;
using ReceiptReader.Application.RawTextExtractors;
using ReceiptReader.Infrastructure.Configurations;

namespace ReceiptReader.Infrastructure.RawTextExtractors
{
    public class AzureAIDocumentIntelligenceRawTextExtractor : IHighCostRawTextExtractor
    {
        private readonly DocumentAnalysisClient _client;
        private readonly ILogger<AzureAIDocumentIntelligenceRawTextExtractor> _logger;
        private readonly AzureAIDocumentIntelligenceOptions _options;

        public AzureAIDocumentIntelligenceRawTextExtractor(
            ILogger<AzureAIDocumentIntelligenceRawTextExtractor> logger,
            IOptions<AzureAIDocumentIntelligenceOptions> options)
        {
            _logger = logger;
            _options = options.Value;
            _client = new DocumentAnalysisClient(
                new Uri(_options.Endpoint),
                new AzureKeyCredential(_options.ApiKey)
            );
        }

        public async Task<string> GetRawTextAsync(MemoryStream ms)
        {
            try
            {
                ms.Position = 0;

                AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentAsync(
                    WaitUntil.Completed,
                    _options.ModelId,
                    ms);

                AnalyzeResult result = await operation.WaitForCompletionAsync();

                return result.Content ?? string.Empty;
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, $"Azure Document Intelligence (High-cost OCR) failed with status {ex.Status}.");

                throw new RawTextExtractionException($"High-cost raw text extraction (Azure Document Intelligence) failed: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "High-cost raw text extraction (Azure Document Intelligence) encountered a critical error.");

                throw new RawTextExtractionException("High-cost raw text extraction (Azure Document Intelligence) failed due to a critical error.", ex);
            }
        }
    }
}

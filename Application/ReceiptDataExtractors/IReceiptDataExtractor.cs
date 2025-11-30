namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// Defines the contract for parsing and extracting all structured data from raw OCR text.
    /// This service orchestrates the entire extraction pipeline workflow.
    /// </summary>
    public interface IReceiptDataExtractor
    {
        // Method summary is skipped, as the interface summary defines the contract.
        Task<ExtractionResult> ExtractDataAsync(string rawText);
    }
}

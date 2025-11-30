namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// Defines the contract for parsing and extracting the detailed tax line list from the raw OCR text.
    /// </summary>
    public interface ITaxLineExtractor
    {
        // Method summary is skipped, as the interface summary defines the contract.
        List<TaxLineDto> Extract(string rawText);
    }
}

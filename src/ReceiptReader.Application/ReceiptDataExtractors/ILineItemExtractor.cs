namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// Defines the contract for parsing and extracting the detailed line item list from the raw OCR text.
    /// </summary>
    public interface ILineItemExtractor
    {
        // Method summary is skipped, as the interface summary defines the contract.
        List<LineItemDto> Extract(string rawText);
    }
}

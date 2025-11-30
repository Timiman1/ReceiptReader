namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// Defines the contract for parsing and extracting the main, simple data fields from the raw OCR text.
    /// </summary>
    public interface ISimpleFieldExtractor
    {
        // Method summary is skipped, as the interface summary defines the contract.
        SimpleFieldsDto Extract(string rawText);
    }
}

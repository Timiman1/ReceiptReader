namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// Defines the contract for validating the total extracted data.
    /// </summary>
    public interface ITotalValidator
    {
        // Method summary is skipped, as the interface summary defines the contract.
        ValidationMetadataDto Validate(ExtractionResult result);
    }
}

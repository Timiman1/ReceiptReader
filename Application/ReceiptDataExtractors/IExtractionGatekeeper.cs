namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// Defines the contract for checking if the simple fields meet the minimum requirements before full validation begins.
    /// </summary>
    public interface IExtractionGatekeeper
    {
        // Method summary is skipped, as the interface summary defines the contract.
        void CheckValidity(SimpleFieldsDto simpleFields);
    }
}

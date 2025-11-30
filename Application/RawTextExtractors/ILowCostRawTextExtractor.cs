namespace ReceiptReader.Application.RawTextExtractors
{
    /// <summary>
    /// Defines the contract for handling low-to-medium quality raw text extraction, often incurring lower cost or no cost.
    /// </summary>
    public interface ILowCostRawTextExtractor : IRawTextExtractor
    {
    }
}

namespace ReceiptReader.Application.RawTextExtractors
{
    /// <summary>
    /// Defines the contract for performing OCR and transform a document stream (image or PDF) into a single raw text string.
    /// </summary>
    public interface IRawTextExtractor
    {
        // Method summary is skipped, as the interface summary defines the contract.
        Task<string> GetRawTextAsync(MemoryStream ms);
    }
}

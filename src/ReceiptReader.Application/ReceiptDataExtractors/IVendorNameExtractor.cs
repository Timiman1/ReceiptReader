namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// Defines a contract for reliably extracting a vendor name from the raw OCR text.
    /// Designed to support advanced or custom parsing strategies due to the high
    /// variability in vendor formats.
    /// </summary>
    public interface IVendorNameExtractor
    {
        // Method summary is skipped, as the interface summary defines the contract.
        List<VendorNameDto> Extract(string rawText);
    }
}

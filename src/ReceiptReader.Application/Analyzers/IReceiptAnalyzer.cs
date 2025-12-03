namespace ReceiptReader.Application.Analyzers
{
    /// <summary>
    /// Defines the contract for analysing raw receipt files and extracting structured data.
    /// This service handles the full analysis workflow, including validation, raw text extraction, 
    /// structured data extraction, and caching.
    /// </summary>
    public interface IReceiptAnalyzer
    {
        // Method summary is skipped, as the interface summary defines the contract.
        Task<AnalysisResult> AnalyzeAsync(Stream stream, Guid fileId);
    }
}

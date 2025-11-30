using ReceiptReader.Domain;

namespace ReceiptReader.Application.Analyzers
{
    /// <summary>
    /// The final result DTO from the <see cref="IReceiptAnalyzer"/>, containing the fully structured <see cref="ReceiptInfo"/> and a flag indicating if the result was <see cref="IsCached"/>.
    /// </summary>
    public class AnalysisResult
    {
        public ReceiptInfo? Receipt { get; set; }
        public bool IsCached { get; set; }
    }
}
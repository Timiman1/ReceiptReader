namespace ReceiptReader.Application.Analyzers
{
    /// <summary>
    /// Defines a contract for checking if the raw OCR text contains the keywords 
    /// required to be proccessed by the extraction pipeline workflow.
    /// </summary>
    public interface IReceiptKeywordClassifier
    {
        bool IsReceiptLikely(string rawText);
    }
}

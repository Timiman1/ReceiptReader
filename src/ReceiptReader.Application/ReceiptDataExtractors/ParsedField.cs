namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// A generic data structure used to wrap an extracted field's <see cref="Value"/> with its <see cref="Confidence"/> score and the <see cref="SourceText"/> used for extraction.
    /// </summary>
    public class ParsedField<T>
    {
        public T? Value { get; set; } = default;

        public double Confidence { get; set; }

        public string? SourceText { get; set; }
    }
}

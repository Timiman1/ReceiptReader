namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// This DTO is basically a smart copy of the <see cref="Domain.ReceiptInfo"/> domain model.
    /// We use this setup to decouple the extraction pipeline from the DB layer. It ensures that the
    /// analysis feature won't break if the persistance changes.
    /// </summary>
    public record ExtractionResult
    {
        public required string RawText { get; init; } = string.Empty;

        public required ParsedField<string?> VendorName { get; init; } = new();
        public required ParsedField<decimal?> TotalAmount { get; init; } = new();
        public required ParsedField<DateTime?> TransactionDate {  get; init; } = new();
        public required ParsedField<string?> Currency { get; init; } = new();

        public required List<LineItemDto> LineItems { get; init; } = [];
        public required List<TaxLineDto> TaxLines { get; init; } = [];

        // Analysis Metadata
        public required ParsedField<decimal?> TaxAmount { get; init; } = new();
        public required bool TotalValidationSuccess { get; init; } = false;
        public required string ValidationMessage {  get; init; } = string.Empty;
    }
}

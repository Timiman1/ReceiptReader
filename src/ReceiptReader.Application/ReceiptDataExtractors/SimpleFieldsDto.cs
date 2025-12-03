namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// This DTO is used by by the <see cref="ISimpleFieldExtractor"/> to return extracted simple fields and their metadata to the orchestrator <see cref="IReceiptDataExtractor"/>.
    /// </summary>
    public record SimpleFieldsDto
    {
        public required ParsedField<string?> VendorName { get; init; } = new();
        public required ParsedField<decimal?> TotalAmount { get; init; } = new();
        public required ParsedField<DateTime?> TransactionDate { get; init; } = new();
        public required ParsedField<string?> Currency { get; init; } = new();
    }
}

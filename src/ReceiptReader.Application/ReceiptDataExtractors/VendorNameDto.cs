namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// This DTO is used by by the <see cref="IVendorNameExtractor"/> to return extracted vendor name and its metadata to the sub-orchestrator <see cref="ISimpleFieldExtractor"/>.
    /// </summary>
    public record VendorNameDto
    {
        public required ParsedField<string?> VendorName { get; init; } = new();
    }
}

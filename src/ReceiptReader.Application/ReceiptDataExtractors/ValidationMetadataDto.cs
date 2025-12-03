namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// This DTO is used by the <see cref="ITotalValidator"/> to report diagnostics back to the orchestrator <see cref="IReceiptDataExtractor"/> and its caller.
    /// </summary>
    public record ValidationMetadataDto
    {
        public required ParsedField<decimal?> VerifiedTaxAmount { get; init; } = new();

        public required bool Success { get; init; } = false;
        public required string Message { get; init; } = string.Empty;
    }
}

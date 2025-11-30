namespace ReceiptReader.Application.ReceiptDataExtractors
{
    /// <summary>
    /// This DTO is basically a smart copy of the <see cref="Domain.ReceiptTaxLine"/> domain model.
    /// We use this setup to decouple the extraction pipeline from the DB layer. It ensures that the
    /// analysis feature won't break if the persistance changes.
    /// </summary>
    public record TaxLineDto(
        decimal TaxAmount,
        decimal TaxableAmount,
        decimal Percentage
    );
}

namespace ReceiptReader.Dtos
{
    public class ReceiptDto
    {
        public Guid FileId { get; set; }

        public string VendorName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime? TransactionDate { get; set; } = null;
        public string Currency { get; set; } = string.Empty;
        public decimal? TaxAmount { get; set; } = null;

        public List<ReceiptLineItemDto> LineItems { get; set; } = [];
        public List<ReceiptTaxLineDto> TaxLines { get; set; } = [];

        public string RawText { get; set; } = string.Empty;
    }
}

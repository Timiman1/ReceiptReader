using System.ComponentModel.DataAnnotations;

namespace ReceiptReader.Domain.Entities
{
    public class ReceiptInfo
    {
        [Key]
        public Guid FileId { get; set; }

        public string VendorName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal? TaxAmount { get; set; } = null;

        public string RawText { get; set; } = string.Empty;

        public ICollection<ReceiptLineItem> LineItems { get; set; } = new List<ReceiptLineItem>();
        public ICollection<ReceiptTaxLine> TaxLines { get; set; } = new List<ReceiptTaxLine>();
    }
}

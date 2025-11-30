using System.ComponentModel.DataAnnotations;

namespace ReceiptReader.Domain
{
    public class ReceiptTaxLine
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal TaxAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Percentage { get; set; }

        public Guid ReceiptInfoId { get; set; }
        public ReceiptInfo ReceiptInfo { get; set; } = default!;
    }
}

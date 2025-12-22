using System.ComponentModel.DataAnnotations;

namespace ReceiptReader.Domain.Entities
{
    public class ReceiptLineItem
    {
        [Key]
        public Guid Id {  get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal TotalLineAmount => Math.Round(Quantity * UnitPrice, 2, MidpointRounding.AwayFromZero);
        public string? ProductCode { get; set; }

        public Guid ReceiptInfoId { get; set; }
        public ReceiptInfo ReceiptInfo { get; set; } = default!;
    }
}

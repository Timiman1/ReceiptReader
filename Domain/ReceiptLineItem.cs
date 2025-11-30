using System.ComponentModel.DataAnnotations;

namespace ReceiptReader.Domain
{
    public class ReceiptLineItem
    {
        [Key]
        public Guid Id {  get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal TotalLineAmount { get; set; }
        public string? ProductCode { get; set; }

        public Guid ReceiptInfoId { get; set; }
        public ReceiptInfo ReceiptInfo { get; set; } = default!;
    }
}

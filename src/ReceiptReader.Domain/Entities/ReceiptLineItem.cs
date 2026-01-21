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
        public bool IsValid => !GetValidationErrors().Any();

        public IEnumerable<string> GetValidationErrors()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return "Name is required.";
            }
            if (Quantity == 0)
            {
                yield return "Quantity cannot be zero.";
            }
            if (ReceiptInfoId == null || ReceiptInfoId == Guid.Empty)
            {
                yield return "Line item must be linked to a receipt.";
            }
        }

        public void LinkToReceipt(Guid receiptInfoId)
        {
            if (receiptInfoId == Guid.Empty)
                throw new ArgumentException("Receipt info ID cannot be empty.", nameof(receiptInfoId));

            if (ReceiptInfoId.HasValue && ReceiptInfoId != Guid.Empty)
                throw new InvalidOperationException("This line item is already linked to a receipt.");

            ReceiptInfoId = receiptInfoId;
        }
    }
}

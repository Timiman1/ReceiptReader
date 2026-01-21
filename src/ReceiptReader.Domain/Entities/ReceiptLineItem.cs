using System.ComponentModel.DataAnnotations;

namespace ReceiptReader.Domain.Entities
{
    public class ReceiptLineItem
    {
        [Key]
        public Guid Id {  get; init; } = Guid.NewGuid();

        public string Name { get; init; } = string.Empty;
        public decimal Quantity { get; init; } = 1;
        public decimal UnitPrice { get; init; }
        public decimal TotalLineAmount => Math.Round(Quantity * UnitPrice, 2, MidpointRounding.AwayFromZero);
        public string? ProductCode { get; init; }

        // EF Core can still populate these even if the setter is private
        public Guid? ReceiptInfoId { get; private set; }
        public ReceiptInfo? ReceiptInfo { get; private set; }

        public bool IsDiscount => UnitPrice < 0;

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

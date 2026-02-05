using ReceiptReader.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace ReceiptReader.Domain.Entities
{
    public class ReceiptLineItem
    {
        [Key]
        public Guid Id {  get; init; } = Guid.NewGuid();

        public string Name { get; init; } = string.Empty;
        public decimal Quantity { get; init; } = 1;
        public QuantityType QuantityType { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal TotalLineAmount { get; init; }
        public string? ProductCode { get; init; }

        // EF Core can still populate these even if the setter is private
        public Guid? ReceiptInfoId { get; private set; }
        public ReceiptInfo? ReceiptInfo { get; private set; }

        public bool IsDiscount => UnitPrice < 0;

        public bool IsValid => !GetValidationErrors().Any();

        public IEnumerable<string> GetValidationErrors()
        {
            decimal Round(decimal value)
            {
                return Math.Round(value, 2, MidpointRounding.AwayFromZero);
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return "Name is required.";
            }
            if (Quantity == 0)
            {
                yield return "Quantity cannot be zero.";
            }

            var calculatedTotal = UnitPrice * Quantity;
            if (Math.Abs(TotalLineAmount - Round(UnitPrice * Quantity)) > 0.02m)
            {
                yield return $"Total line amount {TotalLineAmount} deviates too much from {Quantity} * {UnitPrice}.";
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

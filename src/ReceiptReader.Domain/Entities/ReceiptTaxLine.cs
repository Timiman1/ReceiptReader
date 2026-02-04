using System.ComponentModel.DataAnnotations;

namespace ReceiptReader.Domain.Entities
{
    public class ReceiptTaxLine
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        public decimal TaxAmount { get; init; }
        public decimal GrossAmount { get; init; }
        public decimal NetAmount { get; init; }
        public decimal Percentage { get; init; }

        // EF Core can still populate these even the setter is private
        public Guid? ReceiptInfoId { get; private set; }
        public ReceiptInfo? ReceiptInfo { get; private set; }

        public void LinkToReceipt(Guid receiptInfoId)
        {
            if (receiptInfoId == Guid.Empty)
                throw new ArgumentException("Receipt info ID cannot be empty.", nameof(receiptInfoId));

            if (ReceiptInfoId.HasValue && ReceiptInfoId != Guid.Empty)
                throw new InvalidOperationException("This tax line is already linked to a receipt.");

            ReceiptInfoId = receiptInfoId;
        }
    }
}

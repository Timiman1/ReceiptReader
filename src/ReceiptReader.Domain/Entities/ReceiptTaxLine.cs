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

        public bool IsValid => !GetValidationErrors().Any();

        public IEnumerable<string> GetValidationErrors()
        {
            decimal Round(decimal value)
            {
                return Math.Round(value, 2, MidpointRounding.AwayFromZero);
            }

            if (Percentage < 0.0m || Percentage > 1.0m)
            {
                yield return "Tax percentage must be assigned as a factor (e.g. 0.25).";
            }
            if (Math.Abs(TaxAmount) > Math.Abs(GrossAmount))
            {
                yield return "Absolute tax amount cannot be greater than absolute gross amount.";
            }
            if (Math.Abs(NetAmount) > Math.Abs(GrossAmount))
            {
                yield return "Absolute net amount cannot be greater than absolute gross amount.";
            }
            if (Round(NetAmount) != Round(GrossAmount - TaxAmount))
            {
                yield return $"Net amount {NetAmount} must be equal to gross {GrossAmount} minus tax {TaxAmount}.";
            }

            var expectedTax = NetAmount * Percentage;
            if (Round(TaxAmount) - Round(NetAmount * Percentage) > 0.02m)
            {
                yield return $"Tax amount {TaxAmount} deviates too much from from expected {expectedTax}.";
            }
            if (ReceiptInfoId == null || ReceiptInfoId == Guid.Empty)
            {
                yield return "Tax line must be linked to a receipt";
            }
        }

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

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

        /// <summary>
        /// The full text extracted by the OCR engine(s). 
        /// Integrity and extraction confidence are verified by IExtractionGatekeeper 
        /// before this entity is populated.
        /// </summary>
        public string RawText { get; set; } = string.Empty;

        public ICollection<ReceiptLineItem> LineItems { get; set; } = new List<ReceiptLineItem>();
        public ICollection<ReceiptTaxLine> TaxLines { get; set; } = new List<ReceiptTaxLine>();

        public bool IsReadyForPosting => !GetValidationErrors().Any();

        public IEnumerable<string> GetValidationErrors()
        {
            if (!TransactionDate.HasValue)
                yield return "Transaction date is missing.";

            if (TotalAmount <= 0)
                yield return "Total amount must be greater than zero.";

            if (string.IsNullOrWhiteSpace(VendorName))
                yield return "Vendor name is required.";

            if (string.IsNullOrWhiteSpace(Currency))
                yield return "Currency code is required.";

            if (TransactionDate.HasValue)
            {
                if (TransactionDate > DateTime.UtcNow.AddDays(1))
                    yield return "Transaction date cannot be in the future.";

                if (TransactionDate < DateTime.UtcNow.AddYears(-10))
                    yield return "Transaction date is too far in the past.";
            }

        }
    }
}

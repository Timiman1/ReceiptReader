using System.ComponentModel.DataAnnotations;

namespace ReceiptReader.Domain.Entities
{
    public enum AnalysisStatus
    {
        Pending,
        FilteredOut,
        DataExtractionFailed,
        CriticalFailure,
        Completed
    }

    public class AnalysisLog
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        [MaxLength(64)] // Length of a SHA256 hash
        public string FileHash { get; init; } = string.Empty;
        public DateTime AnalysisDate { get; init; } = DateTime.UtcNow;
        public AnalysisStatus Status { get; private set; } = AnalysisStatus.Pending;
        public string FailureReason { get; private set; } = string.Empty;


        // EF Core can still populate these even if the setter is private
        public Guid? ReceiptInfoId { get; private set; }
        public ReceiptInfo? ReceiptInfo { get; private set; }

        public bool IsValid => !GetValidationErrors().Any();

        public IEnumerable<string> GetValidationErrors()
        {
            if (string.IsNullOrWhiteSpace(FileHash))
            {
                yield return "File hash is required.";
            }
            else if (FileHash.Length != 64)
            {
                yield return "File hash must be a valid SHA256 hash.";
            }
            if (ReceiptInfoId == null || ReceiptInfoId == Guid.Empty)
                yield return "Analysis log must be linked to a receipt.";
        }

        public void MarkCompleted()
        {
            if (!IsValid)
                throw new InvalidOperationException("Analysis log cannot be completed in an invalid state.");

            Status = AnalysisStatus.Completed;
            FailureReason = string.Empty;
        }

        public void MarkFailed(AnalysisStatus failureStatus, string reason)
        {
            if (failureStatus == AnalysisStatus.Completed || failureStatus == AnalysisStatus.Pending)
                throw new ArgumentException("Must provide a valid failure status.");

            Status = failureStatus;
            FailureReason = reason;
        }

        public void LinkToReceipt(Guid receiptInfoId)
        {
            if (receiptInfoId == Guid.Empty)
                throw new ArgumentException("Receipt info ID cannot be empty.", nameof(receiptInfoId));

            if (ReceiptInfoId.HasValue && ReceiptInfoId != Guid.Empty)
                throw new InvalidOperationException("This log is already linked to receipt.");

            ReceiptInfoId = receiptInfoId;
        }
    }
}

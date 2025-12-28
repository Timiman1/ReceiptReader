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
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(64)] // Length of a SHA256 hash
        public string FileHash { get; set; } = string.Empty;
        public DateTime AnalysisDate { get; set; } = DateTime.UtcNow;
        public AnalysisStatus Status { get; set; }
        public string FailureReason { get; set; } = string.Empty;

        public Guid? ReceiptInfoId { get; set; }
        public ReceiptInfo? ReceiptInfo { get; set; }

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
    }
}

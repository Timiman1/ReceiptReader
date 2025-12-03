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
    }
}

using ReceiptReader.Domain.Entities;

namespace ReceiptReader.Application.Repositories
{
    /// <summary>
    /// Defines the contract for persisting and fetching <see cref="AnalysisLog"/> domain models using an optional DB context.
    /// </summary>
    public interface IAnalysisLogRepository
    {
        /// <summary>
        /// Summary in progress
        /// </summary>
        Task<AnalysisLog?> GetLogEntryByHashAsync(string fileHash);

        /// <summary>
        /// Summary in progress
        /// </summary>
        Task AddLogAsync(AnalysisLog logEntry);
    }
}

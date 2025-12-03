using ReceiptReader.Domain.Entities;

namespace ReceiptReader.Application.Repositories
{
    /// <summary>
    /// Defines the contract for managing (persisting, fetching, searching, paginating and deleting) <see cref="ReceiptInfo"/> domain models using an optional DB context.
    /// </summary>
    public interface IReceiptRepository
    {
        /// <summary>
        /// Summary in progress
        /// </summary>
        Task AddAsync(ReceiptInfo receipt);

        /// <summary>
        /// Summary in progress
        /// </summary>
        Task<ReceiptInfo?> GetByFileIdAsync(Guid fileId);

        /// <summary>
        /// Summary in progress
        /// </summary>
        Task<(IEnumerable<ReceiptInfo> Receipts, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Summary in progress
        /// </summary>
        Task<IEnumerable<ReceiptInfo>> SearchAsync(string vendorNameQuery);

        /// <summary>
        /// Summary in progress
        /// </summary>
        Task UpdateAsync(ReceiptInfo receipt);

        /// <summary>
        /// Summary in progress
        /// </summary>
        Task DeleteAsync(Guid fileId);
    }
}

using Microsoft.EntityFrameworkCore;
using ReceiptReader.Application.Repositories;
using ReceiptReader.Domain.Entities;
using ReceiptReader.Infrastructure.Data;

namespace ReceiptReader.Infrastructure.Repositories
{
    public class EfReceiptRepository : IReceiptRepository
    {
        private readonly ILogger<EfReceiptRepository> _logger;
        private readonly ReceiptDbContext _db;

        public EfReceiptRepository(
            ILogger<EfReceiptRepository> logger, 
            ReceiptDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task AddAsync(ReceiptInfo receipt)
        {
            _db.Receipts.Add(receipt);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid fileId)
        {
            var receipt = await _db.Receipts
                .FirstOrDefaultAsync(r => r.FileId == fileId);

            if (receipt != null)
            {
                _db.Remove(receipt);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<ReceiptInfo?> GetByFileIdAsync(Guid fileId)
        {
            return await _db.Receipts.FindAsync(fileId);
        }

        public Task<(IEnumerable<ReceiptInfo> Receipts, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReceiptInfo>> SearchAsync(string vendorNameQuery)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(ReceiptInfo receipt)
        {
            var existingReceipt = await _db.Receipts
                .Include(r => r.LineItems)
                .Include(r => r.TaxLines)
                .FirstOrDefaultAsync(r => r.FileId == receipt.FileId);

            if (existingReceipt != null)
            {
                existingReceipt.VendorName = receipt.VendorName;
                existingReceipt.TotalAmount = receipt.TotalAmount;
                existingReceipt.TransactionDate = receipt.TransactionDate;
                existingReceipt.Currency = receipt.Currency;
                existingReceipt.RawText = receipt.RawText;

                existingReceipt.LineItems.Clear();
                foreach (var item in receipt.LineItems)
                {
                    item.ReceiptInfo = existingReceipt;
                    existingReceipt.LineItems.Add(item);
                }

                existingReceipt.TaxLines.Clear();
                foreach (var taxLine in receipt.TaxLines)
                {
                    taxLine.ReceiptInfo = existingReceipt;
                    existingReceipt.TaxLines.Add(taxLine);
                }
            }

            if (_db.ChangeTracker.HasChanges())
            {
                await _db.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation($"Receipt {receipt.FileId} had no changes to save.");
            }
        }
    }
}

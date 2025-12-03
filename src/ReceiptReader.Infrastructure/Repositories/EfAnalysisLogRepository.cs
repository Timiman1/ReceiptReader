using Microsoft.EntityFrameworkCore;
using ReceiptReader.Application.Repositories;
using ReceiptReader.Domain.Entities;
using ReceiptReader.Infrastructure.Data;

namespace ReceiptReader.Infrastructure.Repositories
{
    public class EfAnalysisLogRepository : IAnalysisLogRepository
    {
        private readonly ReceiptDbContext _db;

        public EfAnalysisLogRepository(ReceiptDbContext db)
        {
            _db = db;
        }

        public async Task AddLogAsync(AnalysisLog logEntry)
        {
            _db.AnalysisLogs.Add(logEntry);
            await _db.SaveChangesAsync();
        }

        public async Task<AnalysisLog?> GetLogEntryByHashAsync(string fileHash)
        {
            return await _db.AnalysisLogs
                .AsNoTracking()
                .Include(nameof(ReceiptInfo))
                .OrderByDescending(al => al.AnalysisDate)
                .FirstOrDefaultAsync(al => al.FileHash == fileHash);
        }
    }
}

using ReceiptReader.Application.Commands;
using ReceiptReader.Domain.Shared;
using ReceiptReader.Dtos;

namespace ReceiptReader.Application.Services
{
    /// <summary>
    /// Defines a contract for orchestrating file validation, file storage, receipt analysis, data persistence and cleanup.
    /// </summary>
    public interface IReceiptProcessingService
    {
        Task<ResultT<ReceiptDto>> ProcessReceiptAsync(ProcessReceiptCommand command);
    }
}

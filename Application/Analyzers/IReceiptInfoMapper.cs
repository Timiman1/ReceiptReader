using ReceiptReader.Application.ReceiptDataExtractors;
using ReceiptReader.Domain.Entities;

namespace ReceiptReader.Application.Analyzers
{
    /// <summary>
    /// Defines the contract for transforming a (<see cref="ExtractionResult"/>) object into the final, normalized domain model (<see cref="ReceiptInfo"/>).
    /// </summary>
    public interface IReceiptInfoMapper
    {
        // Method summary is skipped, as the interface summary defines the contract.
        ReceiptInfo MapToDomainModel(ExtractionResult result, Guid fileId);
    }
}

using ReceiptReader.Application.Analyzers;
using ReceiptReader.Application.ReceiptDataExtractors;
using ReceiptReader.Domain;

namespace ReceiptReader.Infrastructure.Analyzers
{
    public class DefaultReceiptInfoMapper : IReceiptInfoMapper
    {
        public ReceiptInfo MapToDomainModel(ExtractionResult result, Guid fileId)
        {
            var receiptInfo = new ReceiptInfo
            {
                FileId = fileId, // Set PK

                // Map required primitives (we know these are non-null due to gatekeeping)
                VendorName = result!.VendorName.Value!,
                TotalAmount = result.TotalAmount.Value!.Value!,
                Currency = result.Currency.Value!,

                // Map optional primitives
                TransactionDate = result.TransactionDate?.Value,
                TaxAmount = result.TaxAmount?.Value,

                // Map Collections
                LineItems = result.LineItems
                    .Select(dto => new ReceiptLineItem
                    {
                        Name = dto.Name,
                        Quantity = dto.Quantity,
                        UnitPrice = dto.UnitPrice,
                        TotalLineAmount = dto.TotalLineAmount,
                        ProductCode = dto.ProductCode
                    }).ToList(),

                TaxLines = result.TaxLines
                    .Select(dto => new ReceiptTaxLine
                    {
                        TaxAmount = dto.TaxAmount,
                        TaxableAmount = dto.TaxableAmount,
                        Percentage = dto.Percentage
                    }).ToList()
            };

            receiptInfo.RawText = result.RawText;

            return receiptInfo;
        }
    }
}

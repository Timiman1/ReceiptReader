using ReceiptReader.Dtos;
using ReceiptReader.Domain.Entities;

namespace ReceiptReader.Mappers
{
    internal static class ReceiptMapper
    {
        public static ReceiptDto ToDto(ReceiptInfo receipt)
        {
            return new ReceiptDto
            {
                FileId = receipt.FileId,

                VendorName = receipt.VendorName ?? string.Empty,
                TotalAmount = receipt.TotalAmount,
                TransactionDate = receipt.TransactionDate,
                Currency = receipt.Currency,
                TaxAmount = receipt.TaxAmount,

                LineItems = ToReceiptLineItemDtos(receipt.LineItems),
                TaxLines = ToReceiptTaxLineDtos(receipt.TaxLines),

                RawText = receipt.RawText,
            };
        }

        private static List<ReceiptLineItemDto> ToReceiptLineItemDtos(ICollection<ReceiptLineItem> lineItems)
        {
            var result = new List<ReceiptLineItemDto>();

            if (lineItems == null || lineItems.Count == 0)
            {
                return result;
            }

            foreach (var lineItem in lineItems)
            {
                result.Add(new ReceiptLineItemDto
                {
                    Name = lineItem.Name,
                    Quantity = lineItem.Quantity,
                    UnitPrice = lineItem.UnitPrice,
                    TotalLineAmount = lineItem.TotalLineAmount,
                    ProductCode = lineItem.ProductCode
                });
            }
            return result;
        }

        private static List<ReceiptTaxLineDto> ToReceiptTaxLineDtos(ICollection<ReceiptTaxLine> taxLines)
        {
            var result = new List<ReceiptTaxLineDto>();
            
            if (taxLines == null || taxLines.Count == 0)
            {
                return result;
            }

            foreach (var taxLine in taxLines)
            {
                result.Add(new ReceiptTaxLineDto
                {
                    TaxAmount = taxLine.TaxAmount,
                    TaxableAmount = taxLine.TaxableAmount,
                    Percentage = taxLine.Percentage,
                });
            }
            return result;
        }
    }
}

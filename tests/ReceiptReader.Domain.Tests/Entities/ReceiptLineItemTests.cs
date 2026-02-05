using ReceiptReader.Domain.Entities;
using ReceiptReader.Domain.Shared;
using Xunit;

namespace ReceiptReader.Domain.Tests.Entities
{
    public class ReceiptLineItemTests
    {
        [Fact]
        public void LineItems_ShouldLinkBackToParentReceipt()
        {
            // Arrange
            var receipt = new ReceiptInfo { FileId = Guid.NewGuid() };
            var item = new ReceiptLineItem
            {
                Name = "Hönökaka"
            };
            item.LinkToReceipt(receipt.FileId);

            // Act
            receipt.LineItems.Add(item);

            // Assert
            var addedItem = receipt.LineItems.First();
            Assert.Equal(receipt.FileId, addedItem.ReceiptInfoId);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var item = new ReceiptLineItem();

            // Assert
            Assert.NotEqual(Guid.Empty, item.Id);

            Assert.Empty(item.Name);
            Assert.Equal(1m, item.Quantity);
            Assert.Equal(QuantityType.Piece, item.QuantityType);
            Assert.Equal(0m, item.UnitPrice);
            Assert.Equal(0m, item.TotalLineAmount);
            Assert.Null(item.ProductCode);
        }
    }
}

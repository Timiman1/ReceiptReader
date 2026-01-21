using ReceiptReader.Domain.Entities;
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
            Assert.Equal(0m, item.UnitPrice);
            Assert.Equal(0m, item.TotalLineAmount);
            Assert.Null(item.ProductCode);
        }

        [Theory]
        // Standard calculations
        [InlineData(2, 2.50, 5.00)]
        [InlineData(3, 10.00, 30.00)]
        [InlineData(0.5, 100.00, 50.00)]
        // Precision and Rounding cases
        [InlineData(1.333, 1.333, 1.78)]
        [InlineData(1.222, 1.222, 1.49)]
        [InlineData(0.125, 1.0, 0.13)]
        public void TotalLineAmount_ShouldCalculateWithTwoDecimalPrecision(
            decimal qty,
            decimal price,
            decimal expectedRoundedTotal)
        {
            // Arrange
            var item = new ReceiptLineItem
            {
                Quantity = qty,
                UnitPrice = price
            };

            // Act
            var actual = item.TotalLineAmount;

            // Assert
            Assert.Equal(expectedRoundedTotal, actual);
        }
    }
}

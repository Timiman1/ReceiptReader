using ReceiptReader.Domain.Entities;
using Xunit;

namespace ReceiptReader.Domain.Tests.Entities
{
    public class ReceiptInfoTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var receipt = new ReceiptInfo();

            // Assert
            Assert.Equal(Guid.Empty, receipt.FileId);

            Assert.NotNull(receipt.LineItems);
            Assert.NotNull(receipt.TaxLines);
            Assert.Empty(receipt.LineItems);
            Assert.Empty(receipt.TaxLines);

            Assert.Empty(receipt.VendorName);
            Assert.Empty(receipt.Currency);
            Assert.Empty(receipt.RawText);

            Assert.Equal(0m, receipt.TotalAmount);
            Assert.Null(receipt.TransactionDate);
            Assert.Null(receipt.TaxAmount);
        }

        [Theory]
        [InlineData("ICA Maxi Halmstad", 150.50, "SEK", 37.62, "Full kvitto-text här")]
        [InlineData("Stora Coop Varberg", 276.90, "KR", null, "Placeholder")]
        [InlineData("Lidl Falkenberg", 186.90, "€", 57.69, "Lorem ipsum")]
        public void Properties_ShouldStoreAndRetrieveValuesCorrectly(
            string vendorName,
            double totalAmount,
            string currency,
            double? taxAmount,
            string rawText)
        {
            // Arrange
            var receipt = new ReceiptInfo();
            var testGuid = Guid.NewGuid();
            var testDate = DateTime.SpecifyKind(new DateTime(2026, 1, 1), DateTimeKind.Utc);

            // Act
            receipt.FileId = testGuid;
            receipt.VendorName = vendorName;
            receipt.TotalAmount = (decimal)totalAmount;
            receipt.Currency = currency;
            receipt.TaxAmount = (decimal?)taxAmount;
            receipt.RawText = rawText;
            receipt.TransactionDate = testDate;

            // Assert
            Assert.Equal(testGuid, receipt.FileId);
            Assert.Equal(vendorName, receipt.VendorName);
            Assert.Equal((decimal)totalAmount, receipt.TotalAmount);
            Assert.Equal(currency, receipt.Currency);
            Assert.Equal((decimal?)taxAmount, receipt.TaxAmount);
            Assert.Equal(rawText, receipt.RawText);
            Assert.Equal(testDate, receipt.TransactionDate);
        }
    }
}

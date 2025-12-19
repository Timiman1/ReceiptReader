using ReceiptReader.Domain.Entities;
using Xunit;

namespace ReceiptReader.Domain.Tests.Entities
{
    public class ReceiptInfoValidationTests
    {
        [Fact]
        public void IsReadyForPosting_ShouldBeFalse_WhenAllFieldsAreEmpty()
        {
            // Arrange & Act
            var receipt = new ReceiptInfo();

            // Assert
            Assert.False(receipt.IsReadyForPosting);
        }

        [Theory]
        [InlineData(null, 10.0, "Någon butik", "$", "Transaction date is missing.")]
        [InlineData("2049-12-31", 0, "ICA Ankeborg", "USD", "Total amount must be greater than zero.")]
        [InlineData("2006-01-01", 10.0, "", "SEK", "Vendor name is required.")]
        [InlineData("2150-01-01", 10.0, "Byggmax Östersund", "", "Currency code is required.")]
        public void GetValidationErrors_ShouldReturnSpecificErrorMessage(
            string? dateStr, 
            decimal amount, 
            string vendor, 
            string currency, 
            string expectedError)
        {
            // Arrange
            var receipt = new ReceiptInfo
            {
                TransactionDate = dateStr != null ? DateTime.Parse(dateStr) : null,
                TotalAmount = amount,
                VendorName = vendor,
                Currency = currency
            };

            // Act
            var errors = receipt.GetValidationErrors();

            // Assert
            Assert.Contains(expectedError, errors);
        }

        [Fact]
        public void GetValidationErrors_ShouldReturnError_WhenDateIsInFuture()
        {
            // Arrange
            var receipt = new ReceiptInfo
            {
                VendorName = "Framtidsbutik",
                TotalAmount = 50.0M,
                Currency = "THB",
                TransactionDate = DateTime.UtcNow.AddYears(1)
            };

            // Act
            var errors = receipt.GetValidationErrors();

            // Assert
            Assert.Contains("Transaction date cannot be in the future.", errors);
        }

        [Fact]
        public void GetValidationErrors_ShouldReturnError_WhenDateIsTooFarInThePast()
        {
            // Arrange
            var receipt = new ReceiptInfo
            {
                VendorName = "Historisk butik",
                TotalAmount = 199.0M,
                Currency = "SEK",
                TransactionDate = DateTime.UtcNow.AddYears(-10)
            };

            // Act
            var errors = receipt.GetValidationErrors();

            // Assert
            Assert.Contains("Transaction date is too far in the past.", errors);
        }
    }
}

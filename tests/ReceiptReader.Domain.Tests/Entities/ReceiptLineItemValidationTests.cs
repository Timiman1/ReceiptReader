using ReceiptReader.Domain.Entities;
using Xunit;

namespace ReceiptReader.Domain.Tests.Entities
{  
    public class ReceiptLineItemValidationTests
    {
        [Theory]
        [InlineData(null, "Name is required.")]
        [InlineData("", "Name is required.")]
        [InlineData("   ", "Name is required.")]
        public void GetValidationErrors_ShouldReturnNamingError_WhenNameIsInvalid(
        string? name,
        string expectedError)
        {
            // Arrange
            var item = new ReceiptLineItem
            {
                Name = name!
            };

            // Act
            var errors = item.GetValidationErrors();

            // Assert
            Assert.Contains(expectedError, errors);
        }

        [Fact]
        public void GetValidationErrors_ShouldReturnReceiptError_WhenReceiptIsMissing()
        {
            // Assert
            var item = new ReceiptLineItem 
            {
                Name = "Messmör"
            };

            // Act
            var errors = item.GetValidationErrors();

            // Arrange
            Assert.Contains("Line item must be linked to a receipt.", errors);
        }

        [Fact]
        public void IsValid_ShouldBeTrue_WhenLineItemIsPerfectlyValid()
        {
            // Arrange
            var item = new ReceiptLineItem 
            { 
                Name = "Lingondricka"
            };
            item.LinkToReceipt(Guid.NewGuid());

            // Act
            var isValid = item.IsValid;

            // Assert
            Assert.True(isValid);
        }

        [Fact]

        public void LinkToReceipt_ShouldThrowException_WhenReceiptInfoIdIsEmpty()
        {
            // Arrange
            var item = new ReceiptLineItem();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => item.LinkToReceipt(Guid.Empty));
            Assert.Contains("Receipt info ID cannot be empty.", ex.Message);
        }

        [Fact]
        public void LinkToReceipt_ShouldThrowException_WhenAlreadyLinkedToAReceipt()
        {
            // Arrange
            var item = new ReceiptLineItem();
            item.LinkToReceipt(Guid.NewGuid());

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => item.LinkToReceipt(Guid.NewGuid()));
            Assert.Contains("This line item is already linked to a receipt.", ex.Message);
        }
    }
}

using ReceiptReader.Domain.Entities;
using Xunit;

namespace ReceiptReader.Domain.Tests.Entities
{
    public class AnalysisLogValidationTests
    {
        [Theory]
        [InlineData(null, "File hash is required.")]
        [InlineData("", "File hash is required.")]
        [InlineData("   ", "File hash is required.")]
        [InlineData("abc", "File hash must be a valid SHA256 hash.")]
        public void GetValidationErrors_ShouldReturnSpecificErrorMessage(
        string? fileHash,
        string expectedError)
        {
            // Arrange
            var log = new AnalysisLog
            {
                FileHash = fileHash ?? string.Empty,
                ReceiptInfoId = Guid.NewGuid()
            };

            // Act
            var errors = log.GetValidationErrors();

            // Assert
            Assert.Contains(expectedError, errors);
        }

        [Theory]
        [InlineData(null, "Analysis log must be linked to a receipt.")]
        [InlineData("00000000-0000-0000-0000-000000000000", "Analysis log must be linked to a receipt.")]
        public void GetValidationErrors_ShouldReturnReceiptError_WhenReceiptIsMissing(
            string? receiptId,
            string expectedError)
        {
            // Assert
            var log = new AnalysisLog
            {
                FileHash = new string('a', 64),
                ReceiptInfoId = receiptId == null ? null : Guid.Parse(receiptId)
            };

            // Act
            var errors = log.GetValidationErrors();

            // Arrange
            Assert.Contains(expectedError, errors);
        }

        [Fact]
        public void MarkCompleted_ShouldThrow_WhenAnalysisLogIsInvalid()
        {
            // Arrange
            var log = new AnalysisLog();

            // Act
            void act() => log.MarkCompleted();

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void MarkCompleted_ShouldSetStatusToCompleted_WhenLogIsPerfectlyValid()
        {
            // Arrange
            var log = new AnalysisLog
            {
                FileHash = new string('a', 64),
                ReceiptInfoId = Guid.NewGuid(),
                Status = AnalysisStatus.Pending,
                FailureReason = "Old error"
            };

            // Act
            log.MarkCompleted();

            // Assert
            Assert.Equal(AnalysisStatus.Completed, log.Status);
            Assert.Equal(string.Empty, log.FailureReason);
        }

        [Fact]
        public void IsValid_ShouldBeTrue_WhenLogIsPerfectlyValid()
        {
            // Arrange
            var log = new AnalysisLog
            {
                FileHash = new string('a', 64),
                ReceiptInfoId = Guid.NewGuid()
            };

            // Act
            var isValid = log.IsValid;

            // Assert
            Assert.True(isValid);
        }
    }
}

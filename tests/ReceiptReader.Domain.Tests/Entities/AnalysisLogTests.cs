using ReceiptReader.Domain.Entities;
using Xunit;

namespace ReceiptReader.Domain.Tests.Entities
{
    public class AnalysisLogTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange
            var expectedTime = DateTime.UtcNow;
            
            // Act
            var log = new AnalysisLog();

            // Assert
            Assert.NotEqual(Guid.Empty, log.Id);

            Assert.Empty(log.FileHash);
            Assert.Equal(expectedTime, log.AnalysisDate, TimeSpan.FromSeconds(1));
            Assert.Equal(AnalysisStatus.Pending, log.Status);
            Assert.Empty(log.FailureReason);

            Assert.Null(log.ReceiptInfoId);
            Assert.Null(log.ReceiptInfo);
        }
    }
}

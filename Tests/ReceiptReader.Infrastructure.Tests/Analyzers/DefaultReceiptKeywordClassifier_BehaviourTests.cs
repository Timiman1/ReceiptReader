using Microsoft.Extensions.Logging;
using Moq;
using ReceiptReader.Infrastructure.Analyzers;
using Xunit;

namespace ReceiptReader.Infrastructure.Tests.Analyzers
{
    public class DefaultReceiptKeywordClassifier_BehaviourTests
    {
        private readonly DefaultReceiptKeywordClassifier _classifier;

        public DefaultReceiptKeywordClassifier_BehaviourTests()
        {
            var loggerMock = new Mock<ILogger<DefaultReceiptKeywordClassifier>>();
            _classifier = new DefaultReceiptKeywordClassifier(loggerMock.Object);
        }

        [Fact]
        public void Should_Not_Classify_As_Receipt_When_Text_Is_Too_Short()
        {
            // Arrange
            string text = "too short";

            // Act
            bool result = _classifier.IsReceiptLikely(text);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_Not_Classify_As_Receipt_When_Text_Has_No_Receipt_Characteristics()
        {
            // Arrange
            string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

            // Act
            bool result = _classifier.IsReceiptLikely(text);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("abc total 250 kr datum 2025-12-01")]
        [InlineData("abc tot 300 belopp 300 SEK 2049/12/31")]
        [InlineData("tot 300 belopp 300 kr datum 2026-01-01")]
        public void Should_Classify_As_Receipt_When_Text_Total_And_Date_Pattern_Appear(string text)
        {
            // Act
            bool result = _classifier.IsReceiptLikely(text);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("Detta är ett kvitto från ICA. Moms: 25% Summa: 200 kr")]
        [InlineData("Kvitto från Coop, belopp 150 kr, datum 2025-12-24")]
        public void Should_Classify_As_Receipt_When_Swedish_Receipt_Patterns_Are_Present(string text)
        {
            // Act
            bool result = _classifier.IsReceiptLikely(text);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("abc total 150 kr lorem ipsum")]
        [InlineData("abc kvitto lorem ipsum 250 kr")]
        [InlineData("kvitto datum 2024-12-01 lorem ipsum")]
        [InlineData("abc moms total 200 kr lorem ipsum")] 
        [InlineData("kvitto moms kassa 250 kr datum 2026-05-05")]
        public void Should_Classify_As_Receipt_When_Valid_Combination_Appear(string text)
        {
            // Act
            bool result = _classifier.IsReceiptLikely(text);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("en kassör som satt i kassan, mottog mastercard och slängde ett kvitto")]
        [InlineData("abc lorem ipsum dolor sit amet 200 kr lorem ipsum")]
        [InlineData("kvitto moms lorem ipsum dolor sit amet debit")]
        public void Should_Not_Classify_As_Receipt_When_Only_Partial_Indicators_Appear(string text)
        {
            // Act
            bool result = _classifier.IsReceiptLikely(text);

            // Assert
            Assert.False(result);
        }
    }
}

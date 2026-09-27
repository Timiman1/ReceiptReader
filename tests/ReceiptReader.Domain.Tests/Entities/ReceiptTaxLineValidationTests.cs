using ReceiptReader.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReceiptReader.Domain.Tests.Entities
{
    public class ReceiptTaxLineValidationTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(-1)]
        [InlineData(1.01)]
        [InlineData(-0.01)]
        public void GetValidationErrors_ShouldReturnSpecificErrorMessage_WhenPercentageIsLessThanZeroOrGreaterThanOne(
            decimal invalidPercentage)
        {
            // Arrange
            var taxLine = new ReceiptTaxLine
            {
                Percentage = invalidPercentage
            };

            // Act
            var errors = taxLine.GetValidationErrors();

            // Assert
            Assert.Contains(errors, e => e.Contains("Tax percentage must be between 0 and 1"));
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(-2, -1)]
        [InlineData(1.01, 1.0)]
        [InlineData(-1.01, -1.0)]
        public void GetValidationErrors_ShouldReturnSpecificErrorMessage_WhenAbsTaxAmountIsGreaterThanAbsGrossAmount(
            decimal tax,
            decimal gross)
        {
            // Arrange
            var taxLine = new ReceiptTaxLine
            {
                TaxAmount = tax,
                GrossAmount = gross
            };

            // Act
            var errors = taxLine.GetValidationErrors();

            // Assert
            Assert.Contains(errors, e => e.Contains("Absolute tax amount cannot be greater than absolute gross amount."));
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(-2, -1)]
        [InlineData(1.01, 1.0)]
        [InlineData(-1.01, -1.0)]
        public void GetValidationErrors_ShouldReturnSpecificErrorMessage_WhenAbsNetAmountIsGreaterThanAbsGrossAmount(
            decimal net,
            decimal gross)
        {
            // Arrange
            var taxLine = new ReceiptTaxLine
            {
                NetAmount = net,
                GrossAmount = gross
            };

            // Act
            var errors = taxLine.GetValidationErrors();

            // Assert
            Assert.Contains(errors, e => e.Contains("Absolute net amount cannot be greater than absolute gross amount."));
        }

        [Theory]
        [InlineData(1, 2, 1.5)]
        [InlineData(1.01, 1.99, 0.58)]
        public void GetValidationErrors_ShouldReturnSpecificErrorMessage_WhenNetAmountIsNotEqualToGrossMinusTax(
            decimal net,
            decimal gross,
            decimal tax)
        {
            // Arrange

            var receipt = new ReceiptInfo();

            var taxLine = new ReceiptTaxLine
            {
                NetAmount = net,
                GrossAmount = gross,
                TaxAmount = tax,
            };
            taxLine.LinkToReceipt(receipt.FileId);

            // Act
            var errors = taxLine.GetValidationErrors();

            // Assert
            Assert.Contains(errors, e => e.Contains("must be equal to gross"));
        }

        [Theory]
        [InlineData(0.3, 1, 0.2)]
        public void GetValidationErrors_ShouldReturnSpecificErrorMessage_WhenTaxDeviatesTooMuchFromExpectedTax(
            decimal tax,
            decimal net,
            decimal percentage)
        {
            // Arrange
            var taxLine = new ReceiptTaxLine
            {
                TaxAmount = tax,
                NetAmount = net,
                Percentage = percentage
            };

            // Act
            var errors = taxLine.GetValidationErrors();

            // Assert
            Assert.Contains(errors, e => e.Contains("deviates too much from expected"));
        }
    }
}

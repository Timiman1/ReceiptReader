using Microsoft.Extensions.Options;
using ReceiptReader.Application.Configurations;
using ReceiptReader.Application.Exceptions;
using ReceiptReader.Application.ReceiptDataExtractors;
using ReceiptReader.Infrastructure.Configurations;

namespace ReceiptReader.Infrastructure.ReceiptDataExtractors
{
    public class DefaultExtractionGateKeeper : IExtractionGatekeeper
    {
        private readonly ILogger<DefaultExtractionGateKeeper> _logger; // WIP
        private readonly double _minConfidence;

        public DefaultExtractionGateKeeper(
            ILogger<DefaultExtractionGateKeeper> logger,
            IOptions<ReceiptDataExtractionOptions> options)
        {
            _logger = logger;
            _minConfidence = options.Value.MinRequiredExtractionConfidence;
        }

        public void CheckValidity(SimpleFieldsDto simpleFields)
        {
            if (simpleFields == null)
            {
                throw new ReceiptDataExtractionException("Simple fields DTO was null after extraction.");
            }

            if (string.IsNullOrWhiteSpace(simpleFields.VendorName?.Value) ||
                simpleFields.VendorName.Confidence < _minConfidence)
            {
                throw new ReceiptDataExtractionException("Vendor name missing or confidence too low.");
            }

            if (simpleFields.TotalAmount?.Value == null ||
                simpleFields.TotalAmount.Confidence < _minConfidence)
            {
                throw new ReceiptDataExtractionException("Total amount missing or confidence too low.");
            }
        }
    }
}

using ReceiptReader.Application.ReceiptDataExtractors;

namespace ReceiptReader.Infrastructure.ReceiptDataExtractors
{
    public class DefaultReceiptDataExtractor : IReceiptDataExtractor
    {
        private readonly ILogger<DefaultReceiptDataExtractor> _logger; // WIP
        private readonly ISimpleFieldExtractor _simpleFieldExtractor;
        private readonly ILineItemExtractor _lineItemExtractor;
        private readonly ITaxLineExtractor _taxLineExtractor;
        private readonly ITotalValidator _totalValidator;
        private readonly IExtractionGatekeeper _extractionGatekeeper;

        public DefaultReceiptDataExtractor(
            ILogger<DefaultReceiptDataExtractor> logger,
            ISimpleFieldExtractor simpleFieldExtractor, 
            ILineItemExtractor lineItemExtractor, 
            ITaxLineExtractor taxLineExtractor, 
            ITotalValidator totalValidator,
            IExtractionGatekeeper extractionGatekeeper)
        {
            _logger = logger;
            _simpleFieldExtractor = simpleFieldExtractor;
            _lineItemExtractor = lineItemExtractor;
            _taxLineExtractor = taxLineExtractor;
            _totalValidator = totalValidator;
            _extractionGatekeeper = extractionGatekeeper;
        }

        public Task<ExtractionResult> ExtractDataAsync(string rawText)
        {
            var simpleFields = _simpleFieldExtractor.Extract(rawText);

            _extractionGatekeeper.CheckValidity(simpleFields);

            var lineItems = _lineItemExtractor.Extract(rawText);
            var taxLines = _taxLineExtractor.Extract(rawText);

            var extractionResult = new ExtractionResult
            {
                RawText = rawText,
                
                VendorName = simpleFields.VendorName!,
                TotalAmount = simpleFields.TotalAmount!,
                TransactionDate = simpleFields.TransactionDate,
                Currency = simpleFields.Currency!,

                LineItems = lineItems,
                TaxLines = taxLines,

                TaxAmount = default!,
                TotalValidationSuccess = false,
                ValidationMessage = string.Empty
            };

            var validationMetaData = _totalValidator.Validate(extractionResult);

            var finalResult = extractionResult with
            {
                TaxAmount = validationMetaData.VerifiedTaxAmount,
                TotalValidationSuccess = validationMetaData.Success,
                ValidationMessage = validationMetaData.Message
            };

            return Task.FromResult(finalResult);
        }
    }
}

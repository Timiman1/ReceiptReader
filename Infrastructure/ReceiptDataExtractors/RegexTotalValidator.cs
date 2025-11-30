using ReceiptReader.Application.ReceiptDataExtractors;
using ReceiptReader.Domain;

namespace ReceiptReader.Infrastructure.ReceiptDataExtractors
{
    public class RegexTotalValidator : ITotalValidator
    {
        private readonly ILogger<RegexTotalValidator> _logger;

        public RegexTotalValidator(ILogger<RegexTotalValidator> logger)
        {
            _logger = logger;
        }

        public ValidationMetadataDto Validate(ExtractionResult result)
        {
            // WIP
            return new ValidationMetadataDto
            {
                VerifiedTaxAmount = result.TaxAmount,
                Success = true,
                Message = "Total Validation Regex logic still in progress."
            };
        }
    }
}

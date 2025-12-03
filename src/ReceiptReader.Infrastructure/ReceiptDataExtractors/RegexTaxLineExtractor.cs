using ReceiptReader.Application.ReceiptDataExtractors;

namespace ReceiptReader.Infrastructure.ReceiptDataExtractors
{
    public class RegexTaxLineExtractor : ITaxLineExtractor
    {
        private readonly ILogger<RegexTaxLineExtractor> _logger; // WIP

        public RegexTaxLineExtractor(ILogger<RegexTaxLineExtractor> logger)
        {
            _logger = logger;
        }

        public List<TaxLineDto> Extract(string rawText)
        {
            // WIP
            return new List<TaxLineDto>();
        }
    }
}

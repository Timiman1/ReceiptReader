using ReceiptReader.Application.ReceiptDataExtractors;

namespace ReceiptReader.Infrastructure.ReceiptDataExtractors
{
    public class RegexLineItemExtractor : ILineItemExtractor
    {
        private readonly ILogger<RegexLineItemExtractor> _logger; // WIP

        public RegexLineItemExtractor(ILogger<RegexLineItemExtractor> logger)
        {
            _logger = logger;
        }

        public List<LineItemDto> Extract(string rawText)
        {
            // WIP
            return new List<LineItemDto>();
        }
    }
}

using ReceiptReader.Application.Analyzers;
using System.Text.RegularExpressions;

namespace ReceiptReader.Infrastructure.Analyzers
{
    public class DefaultReceiptKeywordClassifier : IReceiptKeywordClassifier
    {
        private readonly ILogger<DefaultReceiptKeywordClassifier> _logger; // WIP

        public DefaultReceiptKeywordClassifier(ILogger<DefaultReceiptKeywordClassifier> logger)
        {
            _logger = logger;
        }

        public bool IsReceiptLikely(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText) || rawText.Length < 20)
            {
                // Too short to be a receipt
                return false;
            }

            var lowerText = rawText.ToLowerInvariant().Replace(" ",  "");

            bool foundMiscKeyword = lowerText.Contains("debit") ||
                                    lowerText.Contains("credit") ||
                                    lowerText.Contains("kredit") ||
                                    lowerText.Contains("kvitto") ||
                                    lowerText.Contains("moms") ||
                                    lowerText.Contains("netto") ||
                                    lowerText.Contains("rabatt") ||
                                    lowerText.Contains("kassa") ||
                                    lowerText.Contains("kassör") ||
                                    lowerText.Contains("mastercard") ||
                                    lowerText.Contains("visa");

            bool foundTotalKeyword = lowerText.Contains("total") ||
                                           lowerText.Contains("tot") ||
                                           lowerText.Contains("summa") ||
                                           lowerText.Contains("belopp") ||
                                           lowerText.Contains("kortköp") ||
                                           lowerText.Contains("belopp") ||
                                           lowerText.Contains("brutto");

            bool foundDateKeyword = lowerText.Contains("datum") ||
                                               lowerText.Contains("dat") ||
                                               lowerText.Contains("tid") ||
                                               Regex.Match(rawText, @"\b(\d{4}[-/]\d{2}[-/]\d{2}|\d{2}[-/]\d{2}[-/]\d{4})\b").Success;

            bool foundCurrencyKeyword = lowerText.Contains("kr") ||
                                        lowerText.Contains("sek") ||
                                        lowerText.Contains("$") ||
                                        lowerText.Contains("£");

            if (foundMiscKeyword && 
                (foundTotalKeyword || foundDateKeyword || foundCurrencyKeyword))
            {
                return true;
            }

            if (foundTotalKeyword && foundDateKeyword)
            {
                return true;
            }

            if (foundTotalKeyword && foundCurrencyKeyword)
            {
                return true;
            }

            return false;
        }
    }
}

using ReceiptReader.Application.ReceiptDataExtractors;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ReceiptReader.Infrastructure.ReceiptDataExtractors
{
    /// <summary>
    /// Extracts simple fields (VendorName, TotalAmount, TransactionDate, Currency) using Regex. 
    /// NOTE: This class is still in development (WIP). All Regex patterns, 
    /// vendor lists, and currency lists are currently hardcoded and will be 
    /// moved to the application configuration (IOptions) or local DB for production robustness 
    /// and regional configuration. The injected ILogger will be implemented for failure tracing.
    /// </summary>
    public class RegexSimpleFieldExtractor : ISimpleFieldExtractor
    {
        private readonly ILogger<RegexSimpleFieldExtractor> _logger; // WIP

        public RegexSimpleFieldExtractor(ILogger<RegexSimpleFieldExtractor> logger)
        {
            _logger = logger;
        }

        public SimpleFieldsDto Extract(string rawText)
        {
            var vendorName = ParseVendorName(rawText);
            var totalAmount = ParseTotalAmount(rawText);
            var transactionDate = ParseTransactionDate(rawText);
            var currency = ParseCurrency(rawText);

            return new SimpleFieldsDto
            {
                VendorName = vendorName,
                TotalAmount = totalAmount,
                TransactionDate = transactionDate,
                Currency = currency
            };
        }

        private string[] SplitByNewline(string input)
        {
            string[] separators = new[] { "\r\n", "\r", "\n" };

            return input.Split(
                separators,
                StringSplitOptions.RemoveEmptyEntries
            );
        }

        private string RemoveAfterSubstring(string input, string substring)
        {
            int index = input.IndexOf(substring, StringComparison.OrdinalIgnoreCase);

            if (index >= 0)
            {
                return input.Substring(0, index);
            }

            return input;
        }

        private ParsedField<string?> ParseVendorName(string rawText)
        {
            string[] lines = SplitByNewline(rawText);

            int index = 0;
            while (index < lines.Length)
            {
                var vendorNameLineCandidate = lines[index].Trim();
                var match = Regex.Match(
                    vendorNameLineCandidate, 
                    @"\b(?:ICA|Coop|Willys|Hemköp|Lidl|Systembolaget|City Gross|Pressbyrån|Apoteket|Kronans Apotek|Apotek Hjärtat|Elon|Jula|Biltema|Rusta|ÖoB|Lekia|Gekås Ullared AB)\b", 
                    RegexOptions.IgnoreCase
                );

                if (match.Success == false)
                {
                    index++;
                }
                else
                {
                    var result = vendorNameLineCandidate;

                    var phoneMatch = Regex.Match(
                        result,
                        @"\b(Tel:?\s*\d+|Telefon:?\s*\d+)\b", 
                        RegexOptions.IgnoreCase);

                    if (phoneMatch.Success)
                    {
                        result = RemoveAfterSubstring(result, phoneMatch.Value).Trim();
                    }

                    var postalCodeMatch = Regex.Match(
                        result,
                        @"\b\d{3}\s\d{2}\b",
                        RegexOptions.None);

                    if (postalCodeMatch.Success)
                    {
                        result = RemoveAfterSubstring(result, postalCodeMatch.Value).Trim();
                    }

                    return new ParsedField<string?>
                    {
                        Value = result,
                        Confidence = 1.0,
                        SourceText = vendorNameLineCandidate
                    };
                }
            }

            return new ParsedField<string?>
            {
                Value = null,
                Confidence = 0,
                SourceText = null
            };
        }

        private ParsedField<decimal?> ParseTotalAmount(string rawText)
        {
            var match = Regex.Match(rawText, @"\b(?:Total|Totalt|Summa|Kortköp|Totalt SEK|Köp|SEK|Att betala)\s*(?:\(.*\))?[:\-]?\s*(\d+[.,]\s?\d{2})\b", RegexOptions.IgnoreCase);
            if (match.Success == false)
            {
                return new ParsedField<decimal?> { Value = null, Confidence = 0.0, SourceText = null };
            }

            var totalAmountString = match.Groups[1].Value.Replace(',', '.');
            if (decimal.TryParse(totalAmountString, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
            {
                return new ParsedField<decimal?>
                {
                    Value = amount,
                    Confidence = 1.0,
                    SourceText = match.Value
                };
            }

            return new ParsedField<decimal?> 
            { 
                Value = null, 
                Confidence = 0.0, 
                SourceText = null 
            };
        }

        private ParsedField<string?> ParseCurrency(string rawText)
        {
            var validCurrencies = new[] { "Kr", "SEK", "$", "£" };

            // Regex.Escape ensures symbols like $ and £ are treated as literals
            string pattern = $@"\b({string.Join("|", validCurrencies.Select(Regex.Escape))})\b";
            var match = Regex.Match(rawText, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string matchedCurrency = match.Groups[1].Value;

                return new ParsedField<string?>
                {
                    Value = matchedCurrency,
                    Confidence = 1.0,
                    SourceText = match.Value
                };
            }

            return new ParsedField<string?>
            {
                Value = null,
                Confidence = 0,
                SourceText = null
            };
        }

        private ParsedField<DateTime?> ParseTransactionDate(string rawText)
        {
            var match = Regex.Match(rawText, @"\b(\d{4}[-/]\d{2}[-/]\d{2}|\d{2}[-/]\d{2}[-/]\d{4})\b");

            if (match.Success &&
                DateTime.TryParse(match.Value, out var transactionDate))
            {
                return new ParsedField<DateTime?>
                {
                    Value = transactionDate,
                    Confidence = 1.0,
                    SourceText = match.Value
                };
            }

            return new ParsedField<DateTime?>
            {
                Value = null,
                Confidence = 0,
                SourceText = null
            };
        }
    }
}

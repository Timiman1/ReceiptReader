namespace ReceiptReader.Dtos
{
    public class ParsedFieldDto
    {
        public object? Value { get; set; }
        public double Confidence { get; set; }
        public string? SourceText { get; set; }
    }
}

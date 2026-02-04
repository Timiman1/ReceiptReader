namespace ReceiptReader.Application.Dtos
{
    public class ReceiptTaxLineDto
    {
        public decimal TaxAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Percentage { get; set; }
    }
}

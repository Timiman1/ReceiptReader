namespace ReceiptReader.Dtos
{
    public class ReceiptTaxLineDto
    {
        public decimal TaxAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Percentage { get; set; }
    }
}

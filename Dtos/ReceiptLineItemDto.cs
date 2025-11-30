namespace ReceiptReader.Dtos
{
    public class ReceiptLineItemDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal TotalLineAmount { get; set; }
        public string? ProductCode { get; set; }
    }
}

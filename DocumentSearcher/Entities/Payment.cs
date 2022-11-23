namespace DocumentSearcher.Entities
{
    public class Payment
    {
        public double UnitPrice { get; set; }
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public string Country { get; set; } = null!;
        public string InvoiceNo { get; set; } = null!;
        public string InvoiceDate { get; set; } = null!;
        public string StockCode { get; set; } = null!;
    }
}

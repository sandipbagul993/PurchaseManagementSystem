namespace Services.ViewModels
{
    public class PurchaseOrder
    {
        public int OrderId { get; set; }
        public string? UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

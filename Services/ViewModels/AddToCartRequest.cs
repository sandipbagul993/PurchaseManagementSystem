#nullable disable
namespace Services.ViewModels
{
    public class AddToCartRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int OrderId { get; set; }    
        public int ProductId { get; set; }
        public string Price { get; set; }   
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}

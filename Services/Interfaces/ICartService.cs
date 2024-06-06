using Entities.Models;
using Services.ViewModels;

namespace Services.Interfaces
{
    public interface ICartService
    {
        Task<List<OrderItem>> GetOrderItemsAsync(AddToCartRequest addToCartRequest);
        Task<bool> AddToCartAsync(AddToCartRequest addToCartRequest);
        Task<decimal> CalculateTotalCostAsync(CreatePurchaseOrderRequest createPurchase);
        Task<Order> GeneratePurchaseOrderAsync(CreatePurchaseOrderRequest createPurchase);
        Task<bool> RemoveFromCartAsync(int OrderItemId);
    }
}

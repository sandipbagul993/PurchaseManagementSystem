using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.ViewModels;

namespace Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly PurchaseManagementDBContext _context;
        private static List<OrderItem> cart = new List<OrderItem>();


        public CartService(PurchaseManagementDBContext context)
        {
            _context = context;

        }

        public async Task<bool> AddToCartAsync(AddToCartRequest addToCartRequest)
        {
            var cartOrder = await _context.Orders
               .FirstOrDefaultAsync(o => o.UserId == addToCartRequest.UserId && o.OrderDate == DateTime.Now);

            if (cartOrder == null)
            {
                cartOrder = new Order
                {
                    UserId = addToCartRequest.UserId,
                    OrderDate = DateTime.Now // Indicates it's still in the cart
                };
            }

            await _context.Orders.AddAsync(cartOrder);
            await _context.SaveChangesAsync();

            var Item = new OrderItem
            {
                OrderId = cartOrder.Id,
                ProductId = addToCartRequest.ProductId,
                Quantity = addToCartRequest.Quantity

            };

            await _context.OrderItems.AddAsync(Item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<decimal> CalculateTotalCostAsync(CreatePurchaseOrderRequest createPurchase)
        {
            var cartItems = await _context.OrderItems
               .Include(oi => oi.Product)
               .Where(oi => oi.Order.UserId == createPurchase.UserId)
               .ToListAsync();
            var total = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            return total;
        }

        public async Task<Order> GeneratePurchaseOrderAsync(CreatePurchaseOrderRequest createPurchase)
        {
            var cartOrder = await _context.Orders.AsNoTracking()
                     .Where(o => o.Id == createPurchase.OrderId && o.UserId == createPurchase.UserId).FirstOrDefaultAsync();
            if (cartOrder != null)
            {
                var order = new Order
                {
                    Id = createPurchase.OrderId,
                    UserId = createPurchase.UserId,
                    OrderDate = DateTime.Now,
                    OrderItems = cart.ToList(),
                    Total = await CalculateTotalCostAsync(createPurchase)
                };
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return order;
            }


            return null;

        }

        public async Task<List<OrderItem>> GetOrderItemsAsync(AddToCartRequest addToCartRequest)
        {
            var orderitems = await _context.OrderItems
                          .Include(o => o.Order)
                          .Include(o => o.Product)
                          .Where(c => c.Order.UserId == addToCartRequest.UserId).ToListAsync();

            return orderitems;
        }

        public async Task<bool> RemoveFromCartAsync(int OrderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(OrderItemId);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}

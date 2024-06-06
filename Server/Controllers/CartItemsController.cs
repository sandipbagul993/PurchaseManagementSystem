using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels;


namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartItemsController> _logger;
        public CartItemsController(ICartService cartService, ILogger<CartItemsController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        // GET: api/OrderItems
        [HttpPost("getorderitems")]
        public async Task<ActionResult> GetOrderItems(AddToCartRequest addToCartRequest)
        {
            try
            {
                var orderItems = await _cartService.GetOrderItemsAsync(addToCartRequest);
                //if (orderItems == null || !orderItems.Any())
                //{
                //    return NotFound("Order not found or no items in order.");
                //}
                return Ok(orderItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting items");
                return StatusCode(500, "An error occurred while retrieving the order items.");
            }
        }

        // POST: api/OrderItems

        [HttpPost("AddToCart")]
        public async Task<ActionResult> AddToCart(AddToCartRequest addToCartRequest)
        {        
            try
            {
                if (addToCartRequest == null)
                {
                    return BadRequest("Invalid request. AddToCartRequest cannot be null.");
                }
                var success = await _cartService.AddToCartAsync(addToCartRequest);

                if (!success)
                {
                    _logger.LogError("Failed to add product to cart");
                    return StatusCode(500, new { Message = "Failed to add product to cart" });
                }

                return Ok(new { Message = "Product added successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Car");
                return StatusCode(500, "An error occurred while adding to the cart.");
            }
        }

        [HttpDelete("{orderItemId}")]
        public async Task<IActionResult> RemoveFromCart(int OrderItemId)
        {
            try
            {
                var result = await _cartService.RemoveFromCartAsync(OrderItemId);
                if (!result)
                {
                    return NotFound("Product not found in cart.");
                }
                return Ok("Product removed from cart.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting cart with ID {OrderItemId}");
                return StatusCode(500, "An error occurred while removing the product from the cart.");
            }
        }

        [HttpPost("GeneratePurchaseOrder")]
        public async Task<IActionResult> GeneratePurchaseOrder(CreatePurchaseOrderRequest createPurchase)
        {
            try
            {
                var purchaseOrder = await _cartService.GeneratePurchaseOrderAsync(createPurchase);
                if (purchaseOrder == null)
                {
                    return Ok("An error occurred while generating the purchase order.");
                }
                return Ok(purchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating the purchase order ");
                return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpGet("CalculateTotal")]
        public async Task<IActionResult> CalculateTotal(CreatePurchaseOrderRequest createPurchase)
        {
            try
            {
                var totalCost = await _cartService.CalculateTotalCostAsync(createPurchase);

                return Ok(totalCost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating the total cost ");
                return StatusCode(500, "Internal Server while calculating the total cost.");
            }
        }

    }
}

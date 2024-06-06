using Client.MetaData;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;
using CreatePurchaseOrderRequest = Client.MetaData.CreatePurchaseOrderRequest;

namespace Client.Controllers
{

    [Authorize]
    public class CartController : Controller
    {        
        private readonly UserManager<IdentityUser> _userManager;
        HttpClient client;
        string baseAddress;
        public CartController(IConfiguration configuration, HttpClient client, UserManager<IdentityUser> userManager)
        {
            this.client = new HttpClient();
            baseAddress = configuration["apiAddress"]!;
            this.client.BaseAddress = new Uri(baseAddress);
            _userManager = userManager;           
        }
        // GET: Cart
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            if (userId == null)
            {
                return Unauthorized();
            }

            var addToCartRequest = new AddToCartRequest
            {
                UserId = user?.Id,

            };

            var response = await client.PostAsJsonAsync<AddToCartRequest>
                (baseAddress + "CartItems/getorderitems", addToCartRequest);                   

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                var apiResponse = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<OrderItemViewModel>>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(apiResponse);
            }
            else
            {
                return RedirectToAction("Index", "Products");
            }        
        }     

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var addToCartRequest = new AddToCartRequest
            {
                UserId = user?.Id,
                ProductId = productId,
                Quantity = quantity
            };

            await client.PostAsJsonAsync<AddToCartRequest>(baseAddress + "CartItems/AddToCart", addToCartRequest);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int orderItemId)
        {
            await client.DeleteAsync(baseAddress + $"CartItems/{orderItemId}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePurchaseOrder(int OrderItemId, int OrderId)
        {
            var user = await _userManager.GetUserAsync(User);
            var purchaseOrderRequest = new CreatePurchaseOrderRequest
            {
                UserId = user?.Id,
               OrderId=OrderId,
            };
            //Call Api for Generate Purchase Order
            var response = await client.PostAsJsonAsync<CreatePurchaseOrderRequest>
                (baseAddress + "CartItems/GeneratePurchaseOrder", purchaseOrderRequest);
            var responseData = await response.Content.ReadAsStringAsync();
            var total = JsonConvert.DeserializeObject<OrderViewModel>(responseData);
            RemoveFromCart(OrderItemId);// Delete Ordered Products from Cart
            return View("OrderConfirmation", total);
        }


        [HttpGet]
        public ActionResult OrderConfirmation()
        {
            return View();
        }

    }
}

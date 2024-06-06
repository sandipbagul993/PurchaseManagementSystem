using Azure;
using Client.MetaData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Client.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        HttpClient client;
        string baseAddress;

        public ProductsController(IConfiguration configuration, HttpClient client)
        {
            this.client = new HttpClient();
            baseAddress = configuration["apiAddress"]!;
            this.client.BaseAddress = new Uri(baseAddress);
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var result = await client.GetAsync(baseAddress + "Products");
            if (result.IsSuccessStatusCode)
            {
                var responseData = await result.Content.ReadAsStringAsync();

                var apiResponse = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(responseData, new JsonSerializerOptions
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

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await client.GetFromJsonAsync<ProductViewModel>(baseAddress + $"Products/{id}");
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Products/Create    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                await client.PostAsJsonAsync<ProductViewModel>(baseAddress + "Products", productViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await client.GetFromJsonAsync<ProductViewModel>(baseAddress + $"Products/{id}");
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await client.PutAsJsonAsync<ProductViewModel>(baseAddress + $"Products/{id}", productViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await client.GetFromJsonAsync<ProductViewModel>(baseAddress + $"Products/{id}");

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

           bool result = await client.DeleteFromJsonAsync<bool>(baseAddress + $"Products/{id}");
            if (result==true)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                return StatusCode(500, "Internal Server Error");
            }
           
        }


    }
}

using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Implementations;
using Services.Interfaces;

namespace Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {

            _productService = productService;
            _logger = logger;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();         
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting products");
                return StatusCode(500, "An error occurred while retrieving the products.");
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found");
                    //return NotFound("Product not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting Product with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            try
            {
                var product1 = await _productService.UpdateProductAsync(id, product);
                if (product1 == null)
                {
                    return NotFound("Product not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating Product with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Products

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //Product data = new Product(product.Id = 0, product.Name, product.Price);
            try
            {
                await _productService.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                return StatusCode(500, "An error occurred while adding the product.");
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {

                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

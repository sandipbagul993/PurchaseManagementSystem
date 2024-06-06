using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly PurchaseManagementDBContext _context;

        public ProductService(PurchaseManagementDBContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID '{id}' not found.");
            };
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            //_context.Entry(existingProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return product;
        }
    }
}

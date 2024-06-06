using Entities.Models;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> DeleteProductAsync(int id);
    }
}

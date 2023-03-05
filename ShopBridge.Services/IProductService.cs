using ShopBridge.Domain.Models;

namespace ShopBridge.Services
{
    public interface IProductService
    {
        Task<List<Product>?> GetProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<Product?> AddProductAsync(Product product); 
        Task<Product?> UpdateProductAsync(Product product);
        Task<(bool, string)> DeleteProductAsync(Product product);
    }
}

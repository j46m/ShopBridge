using Microsoft.EntityFrameworkCore;
using ShopBridge.DataAccess.Data;
using ShopBridge.Domain.Models;

namespace ShopBridge.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopDbContext _shopDbContext;

        public ProductService(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }
        public async Task<List<Product>?> GetProductsAsync()
        {
            try
            {
                return await _shopDbContext.Products.ToListAsync();
            }
            catch 
            {
                return null;
            }
        }
        public async Task<Product?> GetProductAsync(int id)
        {
            try
            {
                return await _shopDbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Product?> AddProductAsync(Product product)
        {
            try
            {
                await _shopDbContext.Products.AddAsync(product);
                await _shopDbContext.SaveChangesAsync();

                return await _shopDbContext.Products.FindAsync(product.Id); // Auto ID from DB
            }
            catch 
            {
                return null; 
            }
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            try
            {
                _shopDbContext.Entry(product).State = EntityState.Modified;
                await _shopDbContext.SaveChangesAsync();

                return product;
            }
            catch 
            {
                return null;
            }
        }
        public async Task<(bool, string)> DeleteProductAsync(Product product)
        {
            try
            {
                var dbProduct = await _shopDbContext.Products.FindAsync(product.Id);

                if (dbProduct is null)
                {
                    return (false, "Product could not be found");
                }

                _shopDbContext.Products.Remove(product);
                await _shopDbContext.SaveChangesAsync();

                return (true, "Product got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured deleting product. Error Message: {ex.Message}");
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ShopBridge.Domain.Models;

namespace ShopBridge.DataAccess.Data
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Product> Products{ get; set; }

        public ShopDbContext(DbContextOptions<ShopDbContext> config) : base(config)
        {
            
        }
    }
}

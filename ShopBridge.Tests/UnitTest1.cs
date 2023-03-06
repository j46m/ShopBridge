using Microsoft.EntityFrameworkCore;
using ShopBridge.DataAccess.Data;
using ShopBridge.Domain.Models;
using ShopBridge.Services;

namespace ShopBridge.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {

        //Arrange
        var dbContext = await ReturnFakeData();
        IProductService productService = new ProductService(dbContext);

        //Act        
        var products = await productService.GetProductsAsync();

        //Assert
        Assert.NotNull(products);
        Assert.Equal(2, products.Count);
        Assert.Equal("Test1", products[0].Name);

    }

    private async Task<ShopDbContext> ReturnFakeData()
    {
        var options = new DbContextOptionsBuilder<ShopDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        var context = new ShopDbContext(options);
        context.Database.EnsureCreated();

        context.Products.Add(new Product
        {
            Id = 1,
            ProductCode = "Test1",
            Description = "Description1",
            Name = "Test1",
            Price = 123M,
            Quantity = 12
        });

        context.Products.Add(new Product
        {
            Id = 2,
            ProductCode = "Test2",
            Description = "Description2",
            Name = "Test2",
            Price = 124M,
            Quantity = 10
        });

        await context.SaveChangesAsync();

        return context;
    }
}
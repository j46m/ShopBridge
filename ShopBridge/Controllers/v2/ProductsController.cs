using Microsoft.AspNetCore.Mvc;
using ShopBridge.Domain.Models;
using ShopBridge.Services;
using System.Security.Claims;

namespace ShopBridge.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //To filter product by user
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var products = await _productService.GetProductsAsync();

            if (products is null || products.Count == 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Products in database.");
            }

            return StatusCode(StatusCodes.Status200OK, products);
        }
    }
}

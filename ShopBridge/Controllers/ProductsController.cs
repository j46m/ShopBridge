using Microsoft.AspNetCore.Mvc;
using ShopBridge.Domain.Models;
using ShopBridge.Services;
using System.Security.Claims;

namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Product? product = await _productService.GetProductAsync(id);

            if (product is null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Product found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, product);
        }


        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] Product product)
        {
            var dbProduct = await _productService.AddProductAsync(product);

            if (dbProduct is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{product.Name} could not be added.");
            }

            return CreatedAtAction("GetBook", new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            Product? dbProduct = await _productService.UpdateProductAsync(product);

            if (dbProduct is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{product.Name} could not be updated");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var prodToDelete = await _productService.GetProductAsync(id);

            if (prodToDelete is null)
                return StatusCode(StatusCodes.Status204NoContent, "Product to delete not found in database");

            (bool status, string message) = await _productService.DeleteProductAsync(prodToDelete);

            if (status is false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, prodToDelete);
        }
    }
}

using BusinessLayer.ProductService;
using Entities.Dto_s;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace A2algo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var (statusCode, products) = await _productService.GetAllProductsAsync();

            if (statusCode == HttpStatusCode.OK)
            {
                return Ok(products);
            }

            return StatusCode((int)statusCode);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
        {
            var (statusCode, product) = await _productService.GetProductByIdAsync(id);

            if (statusCode == HttpStatusCode.OK)
            {
                return Ok(product);
            }
            else if (statusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            return StatusCode((int)statusCode);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<int>> CreateProductAsync(ProductDto productDto)
        {
            var (existingStatusCode, existingProducts) = await _productService.GetAllProductsAsync();
            if (existingStatusCode == HttpStatusCode.OK && existingProducts.Any(p => p.Name == productDto.Name))
            {
                return Conflict("A product with the same name already exists.");
            }

            var (Id, createStatusCode) = await _productService.CreateProductAsync(productDto);

            if (createStatusCode == HttpStatusCode.Created)
            {
                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = Id }, Id);
            }

            return StatusCode((int)createStatusCode);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, ProductDto productDto)
        {
            var statusCode = await _productService.UpdateProductAsync(id, productDto);

            if (statusCode == HttpStatusCode.OK)
            {
                return Ok("Product Updated Successfuly"); 
            }
            else if (statusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            return StatusCode((int)statusCode);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var statusCode = await _productService.DeleteProductAsync(id);

            if (statusCode == HttpStatusCode.OK)
            {
                return Ok("Product Deleted Successfuly");
            }
            else if (statusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            return StatusCode((int)statusCode);
        }
    }
}

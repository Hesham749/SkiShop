using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ProductsController(IProductRepository _ProductRepo) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await _ProductRepo.GetProductsAsync(brand, type, sort));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _ProductRepo.GetProductByIdAsync(id);
            if (product is null) return NotFound();
            return product;
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await _ProductRepo.GetBrandsAsync());
        }


        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await _ProductRepo.GetTypesAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _ProductRepo.AddProductAsync(product);
            if (await _ProductRepo.SaveChangesAsync())
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !ProductExists(id))
                return BadRequest("can not update this product");
            _ProductRepo.UpdateProduct(product);
            if (await _ProductRepo.SaveChangesAsync())
                return NoContent();
            return BadRequest("problem updating product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _ProductRepo.GetProductByIdAsync(id);
            if (product is null) return NotFound();
            _ProductRepo.DeleteProduct(product);
            if (await _ProductRepo.SaveChangesAsync())
                return NoContent();
            return BadRequest("problem deleting product");
        }

        bool ProductExists(int id) => _ProductRepo.ProductExists(id);
    }
}

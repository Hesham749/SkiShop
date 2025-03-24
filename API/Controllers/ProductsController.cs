using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications.ProductSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class ProductsController(IGenericRepository<Product> _ProductRepo) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);

            return await CreatePagedResult(_ProductRepo, spec, specParams.PageIndex,
                specParams.PageSize);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _ProductRepo.GetByIdAsync(id);
            if (product is null) return NotFound();
            return product;
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await _ProductRepo.ListAsync(spec));
        }


        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await _ProductRepo.ListAsync(spec));
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _ProductRepo.AddAsync(product);
            if (await _ProductRepo.SaveAllAsync())
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !await ProductExists(id))
                return BadRequest("can not update this product");
            _ProductRepo.Update(product);
            if (await _ProductRepo.SaveAllAsync())
                return NoContent();
            return BadRequest("problem updating product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _ProductRepo.GetByIdAsync(id);
            if (product is null) return NotFound();
            _ProductRepo.Delete(product);
            if (await _ProductRepo.SaveAllAsync())
                return NoContent();
            return BadRequest("problem deleting product");
        }

        async Task<bool> ProductExists(int id) => await _ProductRepo.Exists(id);
    }
}

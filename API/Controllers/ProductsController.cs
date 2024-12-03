using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IGenericRepository<Product> repository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
    {
        var spec = new ProductSpecification(productSpecParams);
        return await CreatePagedResult(repository, spec, productSpecParams.PageIndex, productSpecParams.PageSize);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repository.Add(product);
        if (await repository.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
        return BadRequest("Failed to create product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id)) return BadRequest("Invalid product id");
        repository.Update(product);
        if (await repository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Failed to update product");
    }

    [HttpDelete("{id:int}")]

    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product == null) return NotFound();
        repository.Remove(product);
        if (await repository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Failed to delete product");
    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrandsAsync()
    {
        var spec = new BrandListSpecification();

        return Ok(await repository.ListAsync(spec));
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypesAsync()
    {
        var spec = new TypeListSpecification();
        return Ok(await repository.ListAsync(spec));
    }
    private bool ProductExists(int id)
    {
        return repository.Exists(id);
    }
}

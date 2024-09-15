using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// Controller to manage product-related API actions
public class ProductsController(IGenericRepository<Product> repo) : BaseApiController 
{
    [HttpGet]
    // use ProductSpacParams object instead of all parameters(string? brand, string? type, string? sort)
    // [FromQuery] => if we dont put this our controller hunting for a object from request body
    // so giving controller a hint that this is Query Params from request (see in postmat there is one tab called "Params") not a obect from request body
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
        [FromQuery]ProductSpacParams specParams)
    {
        var spec = new ProductSpecification(specParams);            // Creates a new ProductSpecification to filter products based on brand, type, and sort

        // Pagination
        // var products = await repo.ListAsync(spec);                  // Retrieves products from the repository that match the specification
        // var count = await repo.CountAsync(spec) ;
        // var pagination = new Pagination<Product>(specParams.PageIndex, specParams.PageSize, count, products) ;
        // return Ok(pagination);
        
        // Pagination from base Api
        return await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize); 

        //return Ok(products);                                     // Returns the filtered product list as an HTTP 200 response
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);

        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);  // Returns HTTP 201 with the new product's details
        }

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))              // Validates that the product exists and the ID matches
            return BadRequest("Cannot update this product");

        repo.Update(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();

        repo.Remove(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();                 // Retrieves a distinct list of product brands

        return Ok(await repo.ListAsync(spec));                   // Returns the list of brands as an HTTP 200 response
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();                  // Retrieves a distinct list of product types

        return Ok(await repo.ListAsync(spec));                   // Returns the list of types as an HTTP 200 response
    }

    private bool ProductExists(int id)
    {
        return repo.Exists(id);                                  // Checks if the product exists in the repository by ID
    }
}


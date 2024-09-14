using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// Controller to manage product-related API actions
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase 
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        var spec = new ProductSpecification(brand, type, sort);  // Creates a new ProductSpecification to filter products based on brand, type, and sort

        var products = await repo.ListAsync(spec);               // Retrieves products from the repository that match the specification

        return Ok(products);                                     // Returns the filtered product list as an HTTP 200 response
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);               // Fetches a single product by ID from the repository

        if (product == null) return NotFound();                  // Returns HTTP 404 if the product is not found

        return product;                                          // Returns the product if found
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);                                       // Adds the new product to the repository

        if (await repo.SaveAllAsync())                           // Saves the product to the database
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);  // Returns HTTP 201 with the new product's details
        }

        return BadRequest("Problem creating product");           // Returns HTTP 400 if the product creation fails
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))              // Validates that the product exists and the ID matches
            return BadRequest("Cannot update this product");

        repo.Update(product);                                    // Updates the product in the repository

        if (await repo.SaveAllAsync())                           // Saves the changes
        {
            return NoContent();                                  // Returns HTTP 204 if successful
        }

        return BadRequest("Problem updating the product");       // Returns HTTP 400 if the update fails
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);               // Fetches the product by ID

        if (product == null) return NotFound();                  // Returns HTTP 404 if not found

        repo.Remove(product);                                    // Removes the product from the repository

        if (await repo.SaveAllAsync())                           // Saves the changes
        {
            return NoContent();                                  // Returns HTTP 204 if deletion is successful
        }

        return BadRequest("Problem deleting the product");       // Returns HTTP 400 if deletion fails
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


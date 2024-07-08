using Microsoft.AspNetCore.Mvc;
using MultiTenancyTest.Dtos;
using MultiTenancyTest.Services;

namespace MultiTenancyTest.Controllers;

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
    public async Task<IActionResult> GetAllAsync()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreatedAsync(CreateProductDto dto)
    {
        Product product = new ()
        {
            Name = dto.Name,
            Description = dto.Description,
            Rate = dto.Rate,
        };

        var createdProduct = await _productService.CreatedAsync(product);

        return Ok(createdProduct);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, CreateProductDto dto)
    {
        var existingProduct = await _productService.GetByIdAsync(id); // this use public filter
        if (existingProduct is null)
        {
            return NotFound();
        }

        existingProduct.Name = dto.Name;
        existingProduct.Description = dto.Description;
        existingProduct.Rate = dto.Rate;

        var updatedProduct = await _productService.UpdateAsync(existingProduct);
        return Ok(updatedProduct);
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        await _productService.DeleteAsync(product);

        return NoContent();
    }


}
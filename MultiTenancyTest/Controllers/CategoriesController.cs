using Microsoft.AspNetCore.Mvc;
using MultiTenancyTest.Dtos;
using MultiTenancyTest.Models;
using MultiTenancyTest.Services;
using System.Threading.Tasks;

namespace MultiTenancyTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreatedAsync(CreateCategoryDto dto)
        {
            Category category = new()
            {
                Name = dto.Name
            };

            var createdCategory = await _categoryService.CreatedAsync(category);
            return Ok(createdCategory);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var existingCategory = await _categoryService.GetByIdAsync(id);
            if (existingCategory is null)
            {
                return NotFound();
            }

            existingCategory.Name = dto.Name;

            var updatedCategory = await _categoryService.UpdateAsync(existingCategory);
            return Ok(updatedCategory);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            await _categoryService.DeleteAsync(category);
            return NoContent();
        }
    }
}

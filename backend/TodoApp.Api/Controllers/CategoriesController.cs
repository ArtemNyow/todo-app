using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApp.Api.DTOs;
using TodoApp.Api.Interfaces;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim!);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            int userId = GetUserId();
            var categories = await _categoryService.GetCategoriesAsync(userId);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            int userId = GetUserId();
            var category = await _categoryService.GetCategoryByIdAsync(userId, id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            int userId = GetUserId();
            var category = await _categoryService.CreateCategoryAsync(userId, dto);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            int userId = GetUserId();
            var category = await _categoryService.UpdateCategoryAsync(userId, id, dto);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            int userId = GetUserId();
            var deleted = await _categoryService.DeleteCategoryAsync(userId, id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Data;
using TodoApp.Api.DTOs;
using TodoApp.Api.Interfaces;
using TodoApp.Api.Models;

namespace TodoApp.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync(int userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TaskCount = c.Tasks.Count 
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int userId, int categoryId)
        {
            var category = await _context.Categories
                .Where(c => c.Id == categoryId && c.UserId == userId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TaskCount = c.Tasks.Count
                })
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<CategoryDto> CreateCategoryAsync(int userId, CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                UserId = userId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                TaskCount = 0
            };
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int userId, int categoryId, UpdateCategoryDto dto)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

            if (category == null) return null;

            category.Name = dto.Name;
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                TaskCount = await _context.TaskItems.CountAsync(t => t.CategoryId == categoryId)
            };
        }

        public async Task<bool> DeleteCategoryAsync(int userId, int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
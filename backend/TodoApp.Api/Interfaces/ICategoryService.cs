using TodoApp.Api.DTOs;

namespace TodoApp.Api.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoriesAsync(int userId);
        Task<CategoryDto?> GetCategoryByIdAsync(int userId, int categoryId);
        Task<CategoryDto> CreateCategoryAsync(int userId, CreateCategoryDto dto);
        Task<CategoryDto?> UpdateCategoryAsync(int userId, int categoryId, UpdateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(int userId, int categoryId);
    }
}
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Data;
using TodoApp.Api.DTOs;
using TodoApp.Api.Interfaces;
using TodoApp.Api.Models;

namespace TodoApp.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TaskDto>> GetTasksAsync(int userId, TaskQueryParams queryParams)
        {
            var query = _context.TaskItems
                .Include(t => t.Category) 
                .Where(t => t.UserId == userId); 

            if (queryParams.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == queryParams.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                query = query.Where(t =>
                    t.Title.Contains(queryParams.SearchTerm) ||
                    (t.Description != null && t.Description.Contains(queryParams.SearchTerm)));
            }

            var totalCount = await query.CountAsync();

            var tasks = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    DueDate = t.DueDate,
                    CreatedAt = t.CreatedAt,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category != null ? t.Category.Name : null
                })
                .ToListAsync();

            return new PagedResult<TaskDto>
            {
                Items = tasks,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int userId, int taskId)
        {
            var task = await _context.TaskItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null) return null;

            return MapToDto(task);
        }

        public async Task<TaskDto> CreateTaskAsync(int userId, CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                CategoryId = dto.CategoryId,
                UserId = userId, 
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync(); 

            await _context.Entry(task).Reference(t => t.Category).LoadAsync();

            return MapToDto(task);
        }

        public async Task<TaskDto?> UpdateTaskAsync(int userId, int taskId, UpdateTaskDto dto)
        {
            var task = await _context.TaskItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null) return null; 

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            task.DueDate = dto.DueDate;
            task.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();

            await _context.Entry(task).Reference(t => t.Category).LoadAsync();

            return MapToDto(task);
        }

        public async Task<bool> DeleteTaskAsync(int userId, int taskId)
        {
            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null) return false;

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        private static TaskDto MapToDto(TaskItem task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                CategoryId = task.CategoryId,
                CategoryName = task.Category?.Name
            };
        }
    }
}
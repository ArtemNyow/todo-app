using TodoApp.Api.DTOs;

namespace TodoApp.Api.Interfaces
{
    public interface ITaskService
    {
        Task<PagedResult<TaskDto>> GetTasksAsync(int userId, TaskQueryParams queryParams);
        Task<TaskDto?> GetTaskByIdAsync(int userId, int taskId);
        Task<TaskDto> CreateTaskAsync(int userId, CreateTaskDto dto);
        Task<TaskDto?> UpdateTaskAsync(int userId, int taskId, UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(int userId, int taskId);
    }
}
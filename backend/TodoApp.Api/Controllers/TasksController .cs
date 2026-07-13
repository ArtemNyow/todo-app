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
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim!);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TaskQueryParams queryParams)
        {
            int userId = GetUserId(); // <-- замість int userId = 1;

            var result = await _taskService.GetTasksAsync(userId, queryParams);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            int userId = GetUserId();

            var task = await _taskService.GetTaskByIdAsync(userId, id);
            if (task == null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            int userId = GetUserId();

            var task = await _taskService.CreateTaskAsync(userId, dto);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
        {
            int userId = GetUserId();

            var task = await _taskService.UpdateTaskAsync(userId, id, dto);
            if (task == null) return NotFound();

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            int userId = GetUserId();

            var deleted = await _taskService.DeleteTaskAsync(userId, id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
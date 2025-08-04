using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleTaskManager.Data;
using SimpleTaskManager.Models;
using SimpleTaskManager.DTOs;
using SimpleTaskManager.Services;
using System.ComponentModel.DataAnnotations;

namespace SimpleTaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(AppDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all tasks
        /// </summary>
        /// <returns>List of all tasks</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
        {
            try
            {
                var tasks = await _context.Tasks
                    .OrderByDescending(t => t.CreatedDate)
                    .ToListAsync();
                    
                var taskDtos = TaskMappingService.ToDtos(tasks);
                
                _logger.LogInformation("Retrieved {Count} tasks", tasks.Count);
                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving tasks");
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        /// <summary>
        /// Get a specific task by ID
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>The requested task</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                
                if (task == null)
                {
                    _logger.LogWarning("Task with ID {TaskId} not found", id);
                    return NotFound($"Task with ID {id} not found");
                }

                var taskDto = TaskMappingService.ToDto(task);
                
                _logger.LogInformation("Retrieved task with ID {TaskId}", id);
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving task with ID {TaskId}", id);
                return StatusCode(500, "An error occurred while retrieving the task");
            }
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="createTaskDto">Task to create</param>
        /// <returns>The created task</returns>
        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var task = TaskMappingService.ToEntity(createTaskDto);
                
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                var taskDto = TaskMappingService.ToDto(task);
                
                _logger.LogInformation("Created new task with ID {TaskId}", task.Id);
                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating task");
                return StatusCode(500, "An error occurred while creating the task");
            }
        }

        /// <summary>
        /// Update an existing task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="updateTaskDto">Updated task data</param>
        /// <returns>No content if successful</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingTask = await _context.Tasks.FindAsync(id);
                if (existingTask == null)
                {
                    _logger.LogWarning("Task with ID {TaskId} not found for update", id);
                    return NotFound($"Task with ID {id} not found");
                }

                TaskMappingService.UpdateEntity(existingTask, updateTaskDto);
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Updated task with ID {TaskId}", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await TaskExists(id))
                {
                    return NotFound();
                }
                
                _logger.LogError(ex, "Concurrency error while updating task with ID {TaskId}", id);
                return Conflict("The task was modified by another process");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task with ID {TaskId}", id);
                return StatusCode(500, "An error occurred while updating the task");
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    _logger.LogWarning("Task with ID {TaskId} not found for deletion", id);
                    return NotFound($"Task with ID {id} not found");
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Deleted task with ID {TaskId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task with ID {TaskId}", id);
                return StatusCode(500, "An error occurred while deleting the task");
            }
        }

        /// <summary>
        /// Get tasks by completion status
        /// </summary>
        /// <param name="completed">Filter by completion status</param>
        /// <returns>Filtered list of tasks</returns>
        [HttpGet("status/{completed}")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasksByStatus(bool completed)
        {
            try
            {
                var tasks = await _context.Tasks
                    .Where(t => t.IsComplete == completed)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToListAsync();
                    
                var taskDtos = TaskMappingService.ToDtos(tasks);
                
                _logger.LogInformation("Retrieved {Count} {Status} tasks", tasks.Count, completed ? "completed" : "pending");
                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving tasks by status");
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        /// <summary>
        /// Get tasks by priority
        /// </summary>
        /// <param name="priority">Priority level (1-5)</param>
        /// <returns>Tasks with specified priority</returns>
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasksByPriority(int priority)
        {
            try
            {
                if (priority < 1 || priority > 5)
                {
                    return BadRequest("Priority must be between 1 and 5");
                }

                var tasks = await _context.Tasks
                    .Where(t => t.Priority == priority)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToListAsync();
                    
                var taskDtos = TaskMappingService.ToDtos(tasks);
                
                _logger.LogInformation("Retrieved {Count} tasks with priority {Priority}", tasks.Count, priority);
                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving tasks by priority");
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        /// <summary>
        /// Get overdue tasks
        /// </summary>
        /// <returns>List of overdue tasks</returns>
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetOverdueTasks()
        {
            try
            {
                var now = DateTime.UtcNow;
                var tasks = await _context.Tasks
                    .Where(t => !t.IsComplete && t.DueDate.HasValue && t.DueDate < now)
                    .OrderBy(t => t.DueDate)
                    .ToListAsync();
                    
                var taskDtos = TaskMappingService.ToDtos(tasks);
                
                _logger.LogInformation("Retrieved {Count} overdue tasks", tasks.Count);
                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving overdue tasks");
                return StatusCode(500, "An error occurred while retrieving overdue tasks");
            }
        }

        /// <summary>
        /// Mark a task as complete
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>The updated task</returns>
        [HttpPatch("{id}/complete")]
        public async Task<ActionResult<TaskResponseDto>> CompleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found");
                }

                if (!task.IsComplete)
                {
                    task.IsComplete = true;
                    task.CompletedDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation("Marked task with ID {TaskId} as complete", id);
                }

                var taskDto = TaskMappingService.ToDto(task);
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while completing task with ID {TaskId}", id);
                return StatusCode(500, "An error occurred while completing the task");
            }
        }

        /// <summary>
        /// Get task statistics
        /// </summary>
        /// <returns>Task statistics summary</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetTaskStatistics()
        {
            try
            {
                var totalTasks = await _context.Tasks.CountAsync();
                var completedTasks = await _context.Tasks.CountAsync(t => t.IsComplete);
                var pendingTasks = totalTasks - completedTasks;
                var overdueTasks = await _context.Tasks.CountAsync(t => !t.IsComplete && t.DueDate.HasValue && t.DueDate < DateTime.UtcNow);
                var highPriorityTasks = await _context.Tasks.CountAsync(t => !t.IsComplete && t.Priority >= 4);

                var statistics = new
                {
                    TotalTasks = totalTasks,
                    CompletedTasks = completedTasks,
                    PendingTasks = pendingTasks,
                    OverdueTasks = overdueTasks,
                    HighPriorityTasks = highPriorityTasks,
                    CompletionRate = totalTasks > 0 ? Math.Round((double)completedTasks / totalTasks * 100, 2) : 0
                };

                _logger.LogInformation("Retrieved task statistics");
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving task statistics");
                return StatusCode(500, "An error occurred while retrieving statistics");
            }
        }

        private async Task<bool> TaskExists(int id)
        {
            return await _context.Tasks.AnyAsync(e => e.Id == id);
        }
    }
}

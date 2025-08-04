using SimpleTaskManager.DTOs;
using SimpleTaskManager.Models;

namespace SimpleTaskManager.Services
{
    public static class TaskMappingService
    {
        public static TaskItem ToEntity(CreateTaskDto dto)
        {
            return new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                CreatedDate = DateTime.UtcNow,
                IsComplete = false
            };
        }

        public static void UpdateEntity(TaskItem entity, UpdateTaskDto dto)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.DueDate = dto.DueDate;
            entity.Priority = dto.Priority;
            
            // Handle completion status
            if (dto.IsComplete && !entity.IsComplete)
            {
                entity.CompletedDate = DateTime.UtcNow;
            }
            else if (!dto.IsComplete && entity.IsComplete)
            {
                entity.CompletedDate = null;
            }
            
            entity.IsComplete = dto.IsComplete;
        }

        public static TaskResponseDto ToDto(TaskItem entity)
        {
            return new TaskResponseDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                IsComplete = entity.IsComplete,
                CreatedDate = entity.CreatedDate,
                DueDate = entity.DueDate,
                CompletedDate = entity.CompletedDate,
                Priority = entity.Priority
            };
        }

        public static IEnumerable<TaskResponseDto> ToDtos(IEnumerable<TaskItem> entities)
        {
            return entities.Select(ToDto);
        }
    }
}

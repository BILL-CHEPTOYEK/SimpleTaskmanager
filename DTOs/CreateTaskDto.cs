using System.ComponentModel.DataAnnotations;

namespace SimpleTaskManager.DTOs
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [Range(1, 5)]
        public int Priority { get; set; } = 3;
    }
}

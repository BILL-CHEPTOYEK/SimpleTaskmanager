using System.ComponentModel.DataAnnotations;

namespace SimpleTaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsComplete { get; set; } = false;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? DueDate { get; set; }
        
        public DateTime? CompletedDate { get; set; }
        
        [Range(1, 5)]
        public int Priority { get; set; } = 3; // 1 = Low, 2 = Below Normal, 3 = Normal, 4 = High, 5 = Critical
    }
}

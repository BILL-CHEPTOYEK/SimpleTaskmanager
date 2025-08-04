namespace SimpleTaskManager.DTOs
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int Priority { get; set; }
        public string PriorityText => Priority switch
        {
            1 => "Low",
            2 => "Below Normal",
            3 => "Normal",
            4 => "High",
            5 => "Critical",
            _ => "Unknown"
        };
        public bool IsOverdue => DueDate.HasValue && DueDate < DateTime.UtcNow && !IsComplete;
    }
}

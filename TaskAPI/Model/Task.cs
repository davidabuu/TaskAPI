using TaskAPI.Enums;

namespace TaskAPI.Model
{
    public class Task
    {
        public string? TaskId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskPriority? Priority { get; set; }
        public Enums.TaskStatus? Status { get; set; } 
        public DateTime? DueDate { get; set; }
        public int? ProjectId { get; set; }
    }
}

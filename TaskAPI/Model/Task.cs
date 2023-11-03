
namespace TaskAPI.Model
{
    public class Task
    {
        
        public string? TaskId { get; set; }
        
        public string? Title { get; set; }
        
        public string? Description { get; set; }
        
        public bool? AssignedToManager { get; set; }
        
        public bool? AssignedToEmployee { get; set; }


        public int? MangaerId { get; set; }

        public int? EmployeeId { get; set; }
        
        public Enums.TaskStatus? Status { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}

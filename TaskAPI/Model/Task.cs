using System.ComponentModel.DataAnnotations;
using TaskAPI.Enums;

namespace TaskAPI.Model
{
    public class Task
    {
        [Required]
        public string? TaskId { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public bool? AssignedToManager { get; set; }
        [Required]
        public bool? AssignedToEmployee { get; set; }
        [Required]
        public Enums.TaskStatus? Status { get; set; }
        [Required]
        public DateTime? DueDate { get; set; }
        [Required]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}

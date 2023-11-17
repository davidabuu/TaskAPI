
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskAPI.Model
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        
        public string? Title { get; set; }
        
        public string? Description { get; set; }

        public bool? AssignedToManager { get; set; } = false;

        public bool? AssignedToEmployee { get; set; } = false;


        public int? MangaerId { get; set; }

        public int? EmployeeId { get; set; }
        
        public Enums.TaskStatus? Status { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}

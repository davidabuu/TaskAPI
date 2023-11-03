using Microsoft.EntityFrameworkCore;
using Task = TaskAPI.Model.Task;

namespace TaskAPI.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
     public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
    }
}

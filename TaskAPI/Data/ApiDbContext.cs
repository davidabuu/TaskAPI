using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = TaskAPI.Model.Task;

namespace TaskAPI.Data
{
    public class ApiDbContext : IdentityDbContext //Automatically Adds it to the Database for us thename, role,email, password
    {
        public DbSet<Task> Tasks { get; set; }
     public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
    }
}

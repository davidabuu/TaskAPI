using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskAPI.DTO_s;
using TaskAPI.Model;


namespace TaskAPI.Data
{
    public class ApiDbContext : IdentityDbContext //Automatically Adds it to the Database for us thename, role,email, password
    {
        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Project> Projects { get; set; }
     public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
    }
}

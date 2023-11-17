using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskAPI.Data;
using TaskAPI.Model;

namespace TaskAPI.Core.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApiDbContext context, ILogger logger) : base(context, logger)
        {
        }


        //public async Task<IEnumerable<Project>> GetProjectsAsync()
        //{
          //  try
            //{
             // return await _context.Projects.ToListAsync();
            //}catch (Exception ex) { 
            //throw new Exception(ex.Message);}
        //}

    }
}

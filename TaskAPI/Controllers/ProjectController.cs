using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAPI.Data;
using TaskAPI.Model;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProjectController(ApiDbContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            return Ok(await _context.Projects.ToListAsync());
        }
        [HttpPost]
        [Route("AddNewProject")]
        public async Task<IActionResult> AddNewProject(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return Ok("Project Added was a success");

        }

        [HttpPatch]
        [Route("UpdateAProject")]

        public async Task<IActionResult> UpdateAProject(Project project)
        {
            //Check if project Exists
            var projectExists = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectId == project.ProjectId);
            if (projectExists == null)
            {
                return NotFound("Project Not Found");
            }
            projectExists.AssignedToManager = project.AssignedToManager;
            projectExists.AssignedToEmployee = project.AssignedToEmployee;
            projectExists.Status = project.Status;
            await _context.SaveChangesAsync();

            return Ok("Project Update was a success");
        }

        [HttpDelete]
        [Route("DeleteAProject")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var singleProject = await _context.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);
            if (singleProject == null)
            {
                return NotFound("Project Not Found");
            }
            _context.Projects.Remove(singleProject);
            await _context.SaveChangesAsync();
            return Ok("Project Deleted");
        }

    }
}

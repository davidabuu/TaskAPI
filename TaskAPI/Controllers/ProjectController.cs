using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAPI.Core;
using TaskAPI.Data;
using TaskAPI.Model;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            return Ok(await _unitOfWork.Projects.GetAllAsync());
        }
        [HttpPost]
        [Route("AddNewProject")]
        public async Task<IActionResult> AddNewProject(Project project)
        {
            await _unitOfWork.Projects.Add(project);
            await _unitOfWork.CompletAsync();
            return Ok("Project Added was a success");

        }

        [HttpPatch]
        [Route("UpdateAProject")]

        public async Task<IActionResult> UpdateAProject(Project project)
        {
            //Check if project Exists
            var projectExists = await _unitOfWork.Projects.GetById(project.ProjectId);
            if (projectExists == null)
            {
                return NotFound("Project Not Found");
            }
            await _unitOfWork.Projects.Update(project);
            await _unitOfWork.CompletAsync();

            return Ok("Project Update was a success");
        }

        [HttpDelete]
        [Route("DeleteAProject")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var singleProject = await _unitOfWork.Projects.GetById(id);
            if (singleProject == null)
            {
                return NotFound("Project Not Found");
            }
            _unitOfWork.Projects.Delete(singleProject);
            await _unitOfWork.CompletAsync();
            return Ok("Project Deleted");
        }

    }
}

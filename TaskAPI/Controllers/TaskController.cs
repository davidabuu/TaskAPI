using Microsoft.AspNetCore.Mvc;
using Task = TaskAPI.Model.Task;

namespace TaskAPI.Controllers
{
    public class TaskController : ControllerBase
    {
        private readonly List<Task> _tasks = new List<Task>();

        [HttpGet()]
        [Route("Get All Tasks")]
        public IActionResult GetAllTask()
        {
            return Ok(_tasks);
        }
        [HttpPost()]
        [Route("Add New Task")]
        public IActionResult AddTask(Task task) 
        {
            _tasks.Add(task);
            return Ok();
        }
    }
}

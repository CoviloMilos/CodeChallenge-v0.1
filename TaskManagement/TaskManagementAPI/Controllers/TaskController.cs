using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskRepository _taskRepo;
        
        public TaskController(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery(Name = "prod")] string prod) 
        {
            bool tempProd = false;

            if (!Boolean.TryParse(prod, out tempProd))
            {
                return BadRequest(new { message = "Query parameter prod is not valid."});
            }

            var tasks = await _taskRepo.GetTasks(Boolean.Parse(prod));

            if (tasks.Any())
            {
                return Ok(tasks);
            }
                
            return NoContent();

        }
    }
}
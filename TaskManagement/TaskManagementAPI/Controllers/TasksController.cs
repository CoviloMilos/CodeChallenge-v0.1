using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Interfaces;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private ITaskRepository _taskRepo;
        private IMapper _mapper;
        private ILoggerManager _logger;
        
        public TasksController(ITaskRepository taskRepo, IMapper mapper, ILoggerManager logger)
        {
            _taskRepo = taskRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery(Name = "prod")] string prod) 
        {
            bool tempProd = false;

            if (!Boolean.TryParse(prod, out tempProd))
            {
                _logger.LogInfo(prod == null ? "Query parameter prod isn't specified." : $"Failed in parsing query parameter prod = {prod.ToString()}.");
                return BadRequest(ResponseFormater("Query parameter prod is not valid.", "", "bad-request"));
            }

            _logger.LogInfo("Fetching all the Tasks from the database.");
            var tasks = await _taskRepo.GetTasks(Boolean.Parse(prod));

            if (tasks.Any())
            {
                return Ok(ResponseFormater("Task returned from database.", tasks, "success"));
            }

            _logger.LogInfo("Database is empty"); 
            return NoContent();

        }

        [HttpGet("{taskId}", Name = "GetTask")]
        public async Task<IActionResult> GetTask(int taskId) 
        {   
            _logger.LogInfo($"Fetching Task with id={taskId.ToString()} from the database."); 

            var task = await _taskRepo.GetTaskById(taskId);

            if (task != null)
            {
                return Ok(ResponseFormater("Task returned from database.", task, "success"));
            }

            _logger.LogInfo($"Task by id {taskId.ToString()} doesn't exists");
            return NoContent();
        }

        [HttpGet("compile/{taskId}", Name="GetTaskForCompilation")]
        public async Task<IActionResult> GetTaskForCompilation(int taskId)
        {
            _logger.LogInfo($"Fetching Task with id={taskId.ToString()} from the database.");

            var task = await _taskRepo.GetTaskById(taskId);
            var dtoGetTask = new DtoGetTask();

            _mapper.Map(task, dtoGetTask);

            if (task != null)
            {
                return Ok(ResponseFormater("Task returned from database", dtoGetTask, "success"));
            }

            _logger.LogInfo($"Task by id {taskId.ToString()} doesn't exists");
            return NoContent(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] DtoCreateTask dtoCreateTask) 
        {
            _logger.LogInfo("Creating task...");
            
            var task = _mapper.Map<Models.Task>(dtoCreateTask);
            task.Cases.ToList().ForEach(c => c.TaskGuid = task.TaskGuid);

            Console.WriteLine(JsonConvert.SerializeObject(task, Formatting.Indented));

            await _taskRepo.AddTask(task);

            if (await _taskRepo.SaveAll())
            {
                return CreatedAtRoute("GetTask", new {taskId = 5}, ResponseFormater("Task created.", dtoCreateTask, "success"));
            }   

            _logger.LogError($"Task create failed on save");
            throw new Exception($"Task create failed on save");    
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId) 
        {
            _logger.LogInfo($"Fetching Task with id={taskId.ToString()} from the database."); 

            var task = await _taskRepo.GetTaskById(taskId);

            if (task == null)  
            {
                return NoContent();
            }

            _taskRepo.Delete(task);

            if (await _taskRepo.SaveAll()) 
            {
                return Ok(ResponseFormater("Task deleted successfully.", "", "success"));
            }

            return BadRequest(ResponseFormater("Failed to delete the task", "", "bad-request"));
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] DtoUpdateTask dtoUpdateTask)
        {
            _logger.LogInfo($"Fetching Task with id={taskId.ToString()} from the database."); 

            var taskFromRepo = await _taskRepo.GetTaskById(taskId);

            if (taskFromRepo == null)
            {
                return NoContent();
            }
            
            if (taskFromRepo.IsProdcution == true)
            {
                return BadRequest(ResponseFormater($"Task {taskFromRepo.Name} can't be updated because it's up and running", "", "bad-request"));
            }

            _mapper.Map(dtoUpdateTask, taskFromRepo);
        
            if (await _taskRepo.SaveAll())
            {
                return Ok(ResponseFormater($"Task {taskFromRepo.Name} updated successfully", "", "success"));
            }

            _logger.LogError($"Updating task {taskId} failed on save");
            throw new Exception($"Updating task {taskId} failed on save");
        }

        [HttpPut("case")]
        public async Task<IActionResult> UpdateCase([FromQuery] string taskGuid, int caseNum, [FromBody] DtoUpdateCase dtoUpdateCase)
        {
            _logger.LogInfo($"Fetching Task with TaskGuid={taskGuid.ToString()} and Case with CaseNum={caseNum.ToString()} from the database."); 

            var taskFromRepo = await _taskRepo.GetTaskByGuid(taskGuid);

            if (taskFromRepo == null)
            {
                return NoContent();
            }
            
            if (taskFromRepo.IsProdcution == true)
            {
                return BadRequest(ResponseFormater($"Case {caseNum} can't be updated because it's up and running", "","bad-request"));
            }

            var caseFromRepo = await _taskRepo.GetSpecificCase(taskGuid, caseNum);

            if (caseFromRepo == null)
            {
                return NoContent();
            }
            
            _mapper.Map(dtoUpdateCase, caseFromRepo);


            if (await _taskRepo.SaveAll())
            {
                return Ok(ResponseFormater($"Case updated successfully", "", "success"));
            }

            _logger.LogError($"Updating case {caseNum} failed on save");
            throw new Exception($"Updating case {caseNum} failed on save");
        }

        private object ResponseFormater(string msg, object body, string status)
        {
            var response = new ResponseFormat()
            {
                Message = msg,
                Endpoint = this.HttpContext.Request.Host + this.HttpContext.Request.Path,
                Status = status,
                Body = body
            };

            return new { response };
        }
    }
}
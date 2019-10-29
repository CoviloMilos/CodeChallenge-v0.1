using System;
using System.Threading.Tasks;
using CodeCompilerAPI.Dtos;
using CodeCompilerAPI.Interfaces;
using CodeCompilerAPI.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CodeCompilerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeCompilerController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        private ICompileService _compileService;
        private readonly string taskManagementApiURL;
        public CodeCompilerController(IConfiguration configuration, ICompileService compileService)
        {
            Configuration = configuration;
            taskManagementApiURL = configuration.GetSection("Endpoints:TaskManagementAPI").Value;
            _compileService = compileService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DtoRequestedCode dtoRequestedCode)
        {
            Uri uri = new Uri(taskManagementApiURL);
            var requestAPI = new TaskManagementAPIClient(uri);
            var task = await requestAPI.GetTask(dtoRequestedCode.TaskId.ToString());

            var newTask = _compileService.CompileCode(dtoRequestedCode.Code, task);
            
            return Ok(task);
        }
    }
}
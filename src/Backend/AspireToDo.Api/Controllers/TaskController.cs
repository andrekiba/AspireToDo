using System.Text;
using System.Text.Json;
using AspireToDo.Api.Models;
using AspireToDo.Api.Models.Entities;
using AspireToDo.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace AspireToDo.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    readonly StorageService storageService;
    readonly IDistributedCache cache;
    readonly ILogger<TaskController> logger;

    //public TaskController(StorageService storageService, ILogger<TaskController> logger)
    public TaskController(StorageService storageService, IDistributedCache cache, ILogger<TaskController> logger)
    {
        this.storageService = storageService;
        this.cache = cache;
        this.logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResult>> Get([FromRoute] string id)
    {
        var taskResult = await storageService.GetTask(id);
        return new OkObjectResult(taskResult);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResult>>> GetAll()
    {
        // var tasksResult = await storageService.GetAllTasks();
        // return new OkObjectResult(tasksResult);

        var cachedTasks = await cache.GetAsync("tasks");

        if (cachedTasks is not null)
            return new OkObjectResult(JsonSerializer.Deserialize<IEnumerable<TaskResult>>(cachedTasks));
        
        var tasksResult = await storageService.GetAllTasks();
        await cache.SetAsync("tasks", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tasksResult)), new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddSeconds(10)
        });

        return new OkObjectResult(tasksResult);
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create([FromBody] CreateOrUpdateTask model)
    {
        var id = await storageService.CreateTask(model.Title, model.Description);
        return new OkObjectResult(id);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult> Update([FromRoute] string id, [FromBody] CreateOrUpdateTask model)
    {
        await storageService.UpdateTask(id, model.Title, model.Description);
        return new OkResult();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] string id)
    {
        await storageService.DeleteTask(id);
        return new OkResult();
    }
    
    [HttpPatch("{id}/complete")]
    public async Task<ActionResult> Complete([FromRoute] string id, [FromBody] CompleteTask model)
    {
        await storageService.SetCompleted(id, model.Done);
        return new OkResult();
    }
}
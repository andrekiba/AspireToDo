using AspireToDo.Api.Models;
using Azure;
using Azure.Data.Tables;
using Constants = AspireToDo.Api.Infrastructure.Constants;
using Task = AspireToDo.Api.Models.Entities.Task;

namespace AspireToDo.Api.Services;

public class StorageService
{
    readonly IConfiguration config;
    readonly TableClient taskTable;

    //public StorageService(IConfiguration config, IAzureClientFactory<TableServiceClient> tableServiceClientFactory)
    public StorageService(IConfiguration config, TableServiceClient tableServiceClient)
    {
        this.config = config;
        //var tableServiceClient = tableServiceClientFactory.CreateClient("todoTableClient");
        taskTable = tableServiceClient.GetTableClient(Constants.TaskTable);
    }
    
    public async Task<TaskResult> GetTask(string id)
    {
        await taskTable.CreateIfNotExistsAsync();
        
        var task = (await taskTable.GetEntityAsync<Task>(Constants.TaskPartitionKey, id)).Value;
        return new TaskResult
        {
            Id = task.RowKey,
            Title = task.Title,
            Description = task.Description,
            Done = task.Done
        };
    }
    
    public async Task<IEnumerable<TaskResult>> GetAllTasks()
    {
        await taskTable.CreateIfNotExistsAsync();
        
        var segment = taskTable.QueryAsync<Task>();
        return (await segment.ToListAsync()).Select(x => new TaskResult
        {
            Id = x.RowKey,
            Title = x.Title,
            Description = x.Description,
            Done = x.Done
        });
    }
    
    public async Task<string> CreateTask(string title, string description)
    {
        await taskTable.CreateIfNotExistsAsync();
        
        var task = Task.Create(title, description);
        var response = await taskTable.AddEntityAsync(task);
        return task.RowKey;
    }
    
    public async System.Threading.Tasks.Task UpdateTask(string id, string title, string description)
    {
        var task = (await taskTable.GetEntityAsync<Task>(Constants.TaskPartitionKey, id)).Value;
        task.Update(title, description);
        var response = await taskTable.UpdateEntityAsync(task, ETag.All);
    }
    
    public async System.Threading.Tasks.Task SetCompleted(string id, bool flag)
    {
        var task = (await taskTable.GetEntityAsync<Task>(Constants.TaskPartitionKey, id)).Value;
        task.SetCompleted(flag);
        var response = await taskTable.UpdateEntityAsync(task, ETag.All);
    }
    
    public async System.Threading.Tasks.Task DeleteTask(string id)
    {
        var response = await taskTable.DeleteEntityAsync(Constants.TaskPartitionKey, id);
    }
}
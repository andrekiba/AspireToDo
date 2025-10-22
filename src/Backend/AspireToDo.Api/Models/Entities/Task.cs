using AspireToDo.Api.Infrastructure;
using Azure;
using Azure.Data.Tables;

namespace AspireToDo.Api.Models.Entities;

public class Task : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Done { get; set; }
    
    public Task()
    {
        ETag = ETag.All;
        Done = false;
    }

    public static Task Create(string title, string description)
        => new Task
        {
            PartitionKey = Constants.TaskPartitionKey,
            RowKey = Guid.NewGuid().ToString(),
            Title = title,
            Description = description
        };
    
    public void Update(string title, string description)
    {
        Title = title;
        Description = description;
    }
    
    public void SetCompleted(bool flag)
    {
        Done = flag;
    }
}
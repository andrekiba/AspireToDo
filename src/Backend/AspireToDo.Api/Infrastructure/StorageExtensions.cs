using AspireToDo.Api.Services;
using Microsoft.Extensions.Azure;

namespace AspireToDo.Api.Infrastructure;

public static class StorageExtensions
{
    public static void AddStorage(this IServiceCollection services, IConfiguration config)
    {
        var storageSection = config.GetSection(StorageOptions.SectionName);
        services.Configure<StorageOptions>(storageSection);
            
        var storageOptions = new StorageOptions();
        storageSection.Bind(storageOptions);
        
        services.AddAzureClients(azBuilder =>
        {
            var connectionString = storageOptions.AzureWebJobsStorage;
            azBuilder.AddTableServiceClient(connectionString).WithName("todoTableClient");
        });
        
        services.AddSingleton(typeof(StorageService));
    }

    public static void AddStorage(this IHostApplicationBuilder builder, IConfiguration config)
    {
        var services = builder.Services;

        builder.AddAzureTableServiceClient("tables");

        services.AddSingleton(typeof(StorageService));
    }
}
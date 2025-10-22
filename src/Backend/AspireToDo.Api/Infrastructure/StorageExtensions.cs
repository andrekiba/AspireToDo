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

        builder.AddAzureTableClient("tables");
        //builder.AddAzureBlobClient("blobs");

        /*
        builder.AddAzureTableService("tables", configureSettings: settings =>
        {
            settings.ServiceUri = new Uri("https://todoaspiredevst.table.core.windows.net/");
            settings.HealthChecks = true;
            settings.Tracing = true;
        });

        builder.AddAzureBlobService("blobs", configureSettings: settings =>
        {
            settings.ServiceUri = new Uri("https://todoaspiredevst.blob.core.windows.net/");
            settings.HealthChecks = true;
            settings.Tracing = true;
        });
        */

        services.AddSingleton(typeof(StorageService));
    }
}
using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<IResourceWithConnectionString> tables;

if (builder.ExecutionContext.IsRunMode)
{
    // 1. use emulator
    var storage = builder.AddAzureStorage("storage");
    storage.RunAsEmulator(resourceBuilder => resourceBuilder.WithDataVolume() );
    tables = storage.AddTables("tables");
    
    // 2. use an existing storage account
    //tables = builder.AddConnectionString("tables");
    
    // 3. auto provision a new storage account fluent api
// #pragma warning disable ASPIRE0001
//     var storage = builder.AddAzureStorage("storage", (resourceBuilder, construct, storage) =>
//     {
//         //storage.AssignProperty(p => p.Sku, StorageSkuName.StandardLrs.ToString());
//     });
//     tables = storage.AddTables("tables");
// #pragma warning restore ASPIRE0001

    // 4. auto provision a new storage account bicep
    // var storage = builder.AddBicepTemplate("storage", "storage.bicep")
    //     .WithParameter("...");
    // tables = storage.AddTables("tables");
}
else
{
    var storage = builder.AddAzureStorage("storage");
    tables = storage.AddTables("tables");
}

var api = builder.AddProject<Projects.AspireToDo_Api>("api")
    .WithReference(tables);
    //.WithReference(blobs);

var cache = builder.AddRedis("cache")
    .PublishAsAzureRedis();

api.WithReference(cache);

if (builder.ExecutionContext.IsPublishMode)
{
    var appInsights = builder.Configuration["AppInsights"];
    api.WithEnvironment("APPLICATIONINSIGHTS_CONNECTION_STRING", appInsights);
}

//builder.AddNpmApp("ui", "../../UI")
builder.AddNpmApp("ui", "../../UI", "dev")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(name: "dev", env: "PORT")
    //.WithHttpsEndpoint(name: "dev", env: "PORT")
    .PublishAsDockerFile();

builder.Build().Run();
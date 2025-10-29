using Azure.Provisioning.AppContainers;
using Azure.Provisioning.ContainerRegistry;
using Azure.Provisioning.Expressions;
using Azure.Provisioning.Redis;
using Azure.Provisioning.Storage;
using RedisResource = Azure.Provisioning.Redis.RedisResource;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<IResourceWithConnectionString> tables;
IResourceBuilder<IResourceWithConnectionString> cache;

const string projectName = "aspiretodo";
var env = builder.AddParameter("environmnet");

if (builder.ExecutionContext.IsRunMode)
{
    //1. use emulator
    var storage = builder.AddAzureStorage("storage")
        .RunAsEmulator(azurite =>
    {
        //azurite.WithLifetime(ContainerLifetime.Persistent);
        azurite.WithDataVolume();
    });
    tables = storage.AddTables("tables");

    //2. use an existing storage account as connection string
    //tables = builder.AddConnectionString("tables");
    
    //3. use an existing storage account
    // var resourceGroupName = builder.AddParameter("resourceGroupName");
    // var storageName = builder.AddParameter("storageName");
    // var test = builder.AddAzureStorage("storage").RunAsExisting(storageName, resourceGroupName);

    cache = builder.AddRedis("cache");
}
else
{
    var acr = builder.AddAzureContainerRegistry("acr")
        .ConfigureInfrastructure(infra =>    
        {
            var resources = infra.GetProvisionableResources();
            var acr = resources.OfType<ContainerRegistryService>().Single();
            var envParam = env.AsProvisioningParameter(infra);
            acr.Name = BicepFunction.Interpolate($"{projectName}{envParam}cr").Compile();
        });
    
    var cae = builder.AddAzureContainerAppEnvironment("cae")
        .ConfigureInfrastructure(infra =>    
        {
            var resources = infra.GetProvisionableResources();
            var cae = resources.OfType<ContainerAppManagedEnvironment>().Single();
            var envParam = env.AsProvisioningParameter(infra);
            cae.Name = BicepFunction.Interpolate($"{projectName}-{envParam}-cae").Compile();
        })
        .WithAzureContainerRegistry(acr);
    
    //3. auto provision a new storage account fluent api
    var storage = builder.AddAzureStorage("storage")
        .ConfigureInfrastructure(infra =>
        {
            var storageAccount = infra.GetProvisionableResources().OfType<StorageAccount>().Single();
            var envParam = env.AsProvisioningParameter(infra);
            storageAccount.Name = BicepFunction.Interpolate($"{projectName}{envParam}st").Compile();
            storageAccount.AccessTier = StorageAccountAccessTier.Hot;
            storageAccount.Sku = new StorageSku { Name = StorageSkuName.StandardLrs };
        });
    tables = storage.AddTables("tables");
    
    cache = builder.AddAzureRedis("cache")
        .ConfigureInfrastructure(infra =>
        {
            var redisCache = infra.GetProvisionableResources().OfType<RedisResource>().Single();
            var envParam = env.AsProvisioningParameter(infra);
            redisCache.Name = BicepFunction.Interpolate($"{projectName}-{envParam}-redis").Compile();
            redisCache.Sku = new RedisSku
            {
                Name = RedisSkuName.Basic,
                Family = RedisSkuFamily.BasicOrStandard,
                Capacity = 1
            };
        });
    // var cache = builder.AddRedis("cache")
    // .WithAnnotation(new ManifestPublishingCallbackAnnotation(
    // context => context.Writer.WriteString("type", "azure.redis.cache")));
}

var api = builder.AddProject<Projects.AspireToDo_Api>("api")
    .WithReference(cache)
    .WithReference(tables)
    .WaitFor(cache)
    .WaitFor(tables)
    .PublishAsAzureContainerApp((infra, app) =>
    {
        var envParam = env.AsProvisioningParameter(infra);
        app.Name = BicepFunction.Interpolate($"{projectName}-{envParam}-api").Compile();
    });

if (builder.ExecutionContext.IsPublishMode)
{
    var appInsights = builder.Configuration["AppInsights"];
    api.WithEnvironment("APPLICATIONINSIGHTS_CONNECTION_STRING", appInsights);
}

builder.AddNpmApp("ui", "../../UI", "start")
    .WithNpmPackageInstallation()
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();
    // .PublishAsAzureContainerApp((infra, app) =>
    // {
    //     var envParam = env.AsProvisioningParameter(infra);
    //     app.Name = BicepFunction.Interpolate($"{projectName}-{envParam}-ui").Compile();
    // });

builder.Build().Run();
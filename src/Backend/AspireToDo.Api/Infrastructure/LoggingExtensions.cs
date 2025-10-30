using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace AspireToDo.Api.Infrastructure;

public static class LoggingExtensions
{
    public static void AddLogger(this IServiceCollection services, IConfiguration config)
    {
        var logger = new LoggerConfiguration()
#if DEBUG
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
            .MinimumLevel.Information()
#else
            .WriteTo.AzureTableStorage(config.GetValue<string>("ConnectionStrings:tables"), storageTableName: "ToDoLog")
            .MinimumLevel.Error()
#endif
            .CreateLogger();

        services.AddLogging(lb => lb.AddSerilog(logger));
    }
}
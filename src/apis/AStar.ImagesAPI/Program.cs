using AStar.ASPNet.Extensions.PipelineExtensions;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;
using AStar.ASPNet.Extensions.WebApplicationBuilderExtensions;
using AStar.Logging.Extensions;
using FastEndpoints;
using Microsoft.OpenApi.Models;
using Serilog;
using FastEndpoints.Swagger;

namespace AStar.ImagesAPI;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        try
        {
            _ = builder.CreateBootstrapLogger("astar-logging-settings.json")
                       .DisableServerHeader()
                       .AddLogging("astar-logging-settings.json");
            var useFastEndpoints = builder.Configuration.GetValue<bool>("apiConfiguration:useFastEndpoints");

            if(useFastEndpoints)
            {
                _ = builder.Services.AddFastEndpoints();
                _ = builder.Services.ConfigureApi(new OpenApiInfo() { Title = "AStar Web Images API", Version = "v1" });
            }
            else
            {
                _ = builder.Services.ConfigureApi(new OpenApiInfo() { Title = "AStar Web Images API", Version = "v1" });
            }

            Log.Information("Starting {AppName}", typeof(Program).AssemblyQualifiedName);
            _ = StartupConfiguration.Services.Configure(builder.Services, builder.Configuration);

            var app = builder.Build().ConfigurePipeline();
            if(useFastEndpoints)
            {
                _ = app.UseFastEndpoints();
            }

            _ = ConfigurePipeline(app);
            app.Run();
        }
        catch(Exception ex)
        {
            Log.Error(ex, "Fatal error occurred in {AppName}", typeof(Program).AssemblyQualifiedName);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static WebApplication ConfigurePipeline(WebApplication app)
        // Additional configuration can be performed here
        => app;
}

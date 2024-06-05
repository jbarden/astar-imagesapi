using AStar.ASPNet.Extensions.PipelineExtensions;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;
using AStar.ASPNet.Extensions.WebApplicationBuilderExtensions;
using AStar.Logging.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;

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
                       .AddLogging("astar-logging-settings.json")
                       .Services.ConfigureApi(new OpenApiInfo() { Title = "AStar Web Images API", Version = "v1" });

            Log.Information("Starting {AppName}", typeof(Program).AssemblyQualifiedName);
            _ = StartupConfiguration.Services.Configure(builder.Services, builder.Configuration);

            var app = builder.Build()
                             .ConfigurePipeline();

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

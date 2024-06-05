using System.IO.Abstractions;
using AStar.ASPNet.Extensions.PipelineExtensions;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;
using AStar.ImagesAPI.ApiClients;
using AStar.ImagesAPI.Models;
using AStar.ImagesAPI.Services;
using AStar.Infrastructure.Data;
using AStar.Logging.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
                       .AddLogging("astar-logging-settings.json")
                       .Services.ConfigureApi(new OpenApiInfo() { Title = "AStar Web Images API", Version = "v1" });

            Log.Information("Starting {AppName}", typeof(Program).AssemblyQualifiedName);

            _ = ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            _ = app.ConfigurePipeline();
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

    private static IServiceCollection ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        var  contextOptions = new DbContextOptionsBuilder<FilesContext>()
            .UseSqlite(configuration.GetConnectionString("FilesDb")!)
            .Options;

        _ = services.AddScoped(_ => new FilesContext(contextOptions));
        _ = services
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<IImageService, ImageService>();

        _ = services.Configure<FilesApiConfiguration>(configuration.GetSection("ApiConfiguration:FilesApiConfiguration"));
        _ = services.AddHttpClient<FilesApiClient>().ConfigureHttpClient((serviceProvider, client) =>
                        {
                            client.BaseAddress = serviceProvider.GetRequiredService<IOptions<FilesApiConfiguration>>().Value.BaseUrl;
                            client.DefaultRequestHeaders.Accept.Add(new("application/json"));
                        });

        return services;
    }

    private static WebApplication ConfigurePipeline(WebApplication app)
        // Additional configuration can be performed here
        => app;
}

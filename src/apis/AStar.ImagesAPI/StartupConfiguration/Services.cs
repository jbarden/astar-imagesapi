using System.IO.Abstractions;
using AStar.ImagesAPI.Services;
using AStar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AStar.ImagesAPI.StartupConfiguration;

public static class Services
{
    public static IServiceCollection Configure(IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped(_ => new FilesContext(new() { Value = configuration.GetConnectionString("SqlServer")! }, new() { EnableLogging = false, IncludeSensitiveData = false, InMemory = false }));
        _ = services.AddSingleton<IFileSystem, FileSystem>();
        _ = services.AddSingleton<IImageService, ImageService>();

        var sp = services.BuildServiceProvider();
        var context = sp.GetRequiredService<FilesContext>();
        _ = context.Database.EnsureCreated();

        return services;
    }
}

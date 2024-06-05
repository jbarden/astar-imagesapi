using System.IO.Abstractions;
using AStar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AStar.ImagesAPI.StartupConfiguration;

public static class Services
{
#pragma warning disable IDE0060 // Remove unused parameter

    public static IServiceCollection Configure(IServiceCollection services, IConfiguration configuration)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        var  contextOptions = new DbContextOptionsBuilder<FilesContext>()
            .UseSqlite(configuration.GetConnectionString("FilesDb")!)
            .Options;

        _ = services.AddScoped(_ => new FilesContext(contextOptions));
        _ = services.AddSingleton<IFileSystem, FileSystem>();

        var sp = services.BuildServiceProvider();
        var context = sp.GetRequiredService<FilesContext>();
        _ = context.Database.EnsureCreated();

        return services;
    }
}

using System.IO.Abstractions;
using AStar.ImagesAPI.Extensions;
using AStar.ImagesAPI.Services;
using AStar.Infrastructure.Data;
using Azure;
using Azure.Core;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AStar.ImagesAPI.Images;

public class GetFe(IFileSystem fileSystem, IImageService imageService, FilesContext context, ILogger<Get> logger)
                            : Endpoint<GetRequest, Results<Ok<Stream>, NotFound, ProblemDetails>>
{
    public override void Configure()
    {
        Get("/api/imagesFe");

        // the below are to remind me how to doc and are taken from https://fast-endpoints.com/docs/swagger-support#describe-endpoints
        Description(b => b
            .Accepts<GetRequest>("application/json")
            .Produces<Stream>(200, "application/json")
            .ProducesProblemFE(400) //shortcut for .Produces<ErrorResponse>(400)
            .ProducesProblemFE<InternalErrorResponse>(500),
        clearDefaults: true);
        AllowAnonymous();
    }

    public override async Task<Results<Ok<Stream>, NotFound, ProblemDetails>> ExecuteAsync(GetRequest request, CancellationToken ct)
    {
        var imagePath = request.ImagePath;
        logger.LogDebug("Starting retrieval for the {ImagePath} with maximum dimensions: {MaxDimensions}", imagePath, request.MaxDimensions);
        if(!imagePath.IsImage())
        {
            AddError($"Unsupported file type: {imagePath.Extension()}.");
            return new ProblemDetails(ValidationFailures);
        }

        if(!fileSystem.File.Exists(imagePath))
        {
            return TypedResults.NotFound();
        }

        await UpdateLastViewed(imagePath.DirectoryName(), imagePath.FileName(), ct);
        return null!;// TypedResults.Ok(imageService.GetImage(imagePath, request.MaxDimensions));
    }

    private async Task UpdateLastViewed(string directory, string filename, CancellationToken cancellationToken)
    {
        try
        {
            _ = (await context.Files.Include(file => file.FileAccessDetail)
                                .SingleAsync(f => f.FileName == filename && f.DirectoryName == directory, cancellationToken))
                                .FileAccessDetail.LastViewed = DateTime.UtcNow;
            _ = await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _ = (await context.Files.Include(file => file.FileAccessDetail)
                                .SingleAsync(f => f.FileName == filename && f.DirectoryName == directory, cancellationToken))
                                .FileAccessDetail.LastViewed = DateTime.UtcNow;
            _ = await context.SaveChangesAsync(cancellationToken);
        }
    }
}

using System.IO.Abstractions;
using Ardalis.ApiEndpoints;
using AStar.ImagesAPI.Extensions;
using AStar.ImagesAPI.Models;
using AStar.ImagesAPI.Services;
using AStar.Infrastructure.Data;
using AStar.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using SkiaSharp;

namespace AStar.ImagesAPI.Images;

[Route("api/image")]
public class Get(IFileSystem fileSystem, IImageService imageService, FilesContext context, ILogger<Get> logger) : EndpointBaseAsync
                    .WithRequest<Request>
                    .WithActionResult<FileContentResult>
{
    private const int MaximumHeightAndWidthForThumbnail = 850;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Gets the specified Image",
        Description = "Gets the specified Image and resizes to the specified maximum dimensions",
        OperationId = "Image_get",
        Tags = ["Images"])
]
    public override async Task<ActionResult<FileContentResult>> HandleAsync([FromQuery] Request request, CancellationToken cancellationToken = default)
    {
        var imagePath = request.ImagePath;
        logger.LogDebug("Starting retrieval for the {ImagePath} with maximum dimensions: {MaxDimensions}", imagePath, request.MaxDimensions);
        if(!imagePath.IsImage())
        {
            return BadRequest($"Unsupported file type: {imagePath.Extension()}.");
        }

        if(!fileSystem.File.Exists(imagePath))
        {
            return NotFound();
        }

        await UpdateLastViewed(imagePath.DirectoryName(), imagePath.FileName(), cancellationToken);

        var ffff = new FileContentResult(imageService.GetImage(imagePath, request.MaxDimensions), "image/jpeg");
        return ffff;
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

using System.Drawing;
using System.Runtime.Versioning;
using AStar.ImagesAPI.Models;
using SkiaSharp;
using static System.Net.Mime.MediaTypeNames;

namespace AStar.ImagesAPI.Services;

/// <summary>
///
/// </summary>
/// <param name="logger"></param>
public class ImageService(ILogger<ImageService> logger) : IImageService
{
    private const int MaximumHeightAndWidthForThumbnail = 850;

    /// <inheritdoc/>
    public byte[] GetImage(string imagePath, int maxDimensions)
    {
        logger.LogInformation("Getting resized {ImagePath}", imagePath);
        using var imageFromFile = SKImage.FromEncodedData(imagePath);
        var dimensions = ImageDimensions(imageFromFile.Width, imageFromFile.Height, maxDimensions);
        SKImageInfo info = new SKImageInfo(dimensions.Width, dimensions.Height, SKColorType.Bgra8888);
        SKImage output = SKImage.Create(info);
        _ = imageFromFile.ScalePixels(output.PeekPixels(), SKFilterQuality.None);
        using var data = output.Encode(SKEncodedImageFormat.Jpeg, 50);
        return output.Encode().ToArray();
    }

    private static Dimensions ImageDimensions(int width, int height, int maximumSizeInPixels)
    {
        var restrictedSizeInPixels = maximumSizeInPixels > MaximumHeightAndWidthForThumbnail
                            ? MaximumHeightAndWidthForThumbnail
                            : maximumSizeInPixels;
        var ratio = (double)restrictedSizeInPixels / width;
        var newWidth = (int)(width * ratio);
        var newHeight = (int)(height * ratio);

        return new() { Height = newHeight, Width = newWidth };
    }
}

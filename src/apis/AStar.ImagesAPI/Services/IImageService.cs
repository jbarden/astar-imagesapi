using System.Drawing;
using SkiaSharp;

namespace AStar.ImagesAPI.Services;

/// <summary>
///
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Gets the specified image.
    /// </summary>
    /// <param name="imagePath">The full path to the image.</param>
    /// <param name="maxDimensions">The maximum dimensions that the returned image cannot exceed in either direction.</param>
    /// <returns>The image as a <see cref="Stream"/>.</returns>
    Stream GetImage(string imagePath, int maxDimensions);

    Image GetImage2(string imagePath, int maxDimensions);
}

using System.Drawing;
using System.Runtime.Versioning;

namespace AStar.ImagesAPI.Services;

public class ImageService(ILogger<ImageService> logger) : IImageService
{
    /// <summary>
    /// This feels like it should be on the files API
    /// </summary>
    /// <param name="imagePath">
    /// </param>
    /// <returns>
    /// </returns>
    [SupportedOSPlatform("windows")]
    public Image GetImage(string imagePath)
    {
        try
        {
            return Image.FromFile(imagePath);
        }
        catch(Exception e)
        {
            logger.LogError(e, "An error occurred ({Error}) whilst retrieving {FileName} - full stack: {Stack}", e.Message, imagePath, e.StackTrace);
            var bmp = new Bitmap(100, 50);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.DarkRed);

            return bmp;
        }
    }
}

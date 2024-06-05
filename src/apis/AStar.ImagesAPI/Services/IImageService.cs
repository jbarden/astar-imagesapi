using System.Drawing;

namespace AStar.ImagesAPI.Services;

public interface IImageService
{
    Image GetImage(string imagePath);
}

namespace AStar.ImagesAPI.Extensions;

public static class StringExtensions
{
    public static bool IsImage(this string filename)
        => filename.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
        filename.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
        filename.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
        filename.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
        filename.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase);
}

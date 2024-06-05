namespace AStar.ImagesAPI.Extensions;

public static class FileInfoExtensions
{
    public static bool IsImage(this FileInfo fileInfo)
        => fileInfo.Name.ToUpperInvariant().EndsWith(".JPEG")
        || fileInfo.Name.ToUpperInvariant().EndsWith(".JPG")
        || fileInfo.Name.ToUpperInvariant().EndsWith(".GIF")
        || fileInfo.Name.ToUpperInvariant().EndsWith(".BMP");
}

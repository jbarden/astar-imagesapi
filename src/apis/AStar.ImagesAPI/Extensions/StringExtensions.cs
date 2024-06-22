namespace AStar.ImagesAPI.Extensions;

public static class StringExtensions
{
    public static bool IsImage(this string imagePath)
                            => imagePath.Extension().Equals("jpg", StringComparison.OrdinalIgnoreCase)
                            || imagePath.Extension().Equals("gif", StringComparison.OrdinalIgnoreCase)
                            || imagePath.Extension().Equals("jpeg", StringComparison.OrdinalIgnoreCase)
                            || imagePath.Extension().Equals("png", StringComparison.OrdinalIgnoreCase)
                            || imagePath.Extension().Equals("bmp", StringComparison.OrdinalIgnoreCase);

    public static string Extension(this string imagePath)
                            => imagePath[(imagePath.LastIndexOf('.') + 1)..];

    public static string DirectoryName(this string imagePath)
                            => imagePath[..imagePath.LastIndexOf('\\')];

    public static string FileName(this string imagePath)
                            => imagePath[(imagePath.LastIndexOf('\\')+1)..];
}

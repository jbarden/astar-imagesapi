namespace AStar.ImagesAPI.Images;

public class GetRequest
{
    public string ImagePath { get; set; } = string.Empty;

    public int MaxDimensions { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.Text;
using AStar.ImagesAPI.Models;

namespace AStar.ImagesAPI.Config;

public class SearchParameters
{
    [Required] public string SearchFolder { get; set; } = string.Empty;

    public SearchType SearchType { get; set; }

    public bool RecursiveSubDirectories { get; set; } = true;

    [Required] public int CurrentPage { get; set; } = 1;

    [Required] public int ItemsPerPage { get; set; } = 10;

    [Range(50, 450, ErrorMessage = "Please specify a thumbnail size between 50 and 450 pixels.")]
    public int MaximumSizeOfThumbnail { get; set; } = 150;

    [Range(50, 999999, ErrorMessage = "Please specify an image size between 500 and 999999 (NOT recommended!) pixels.")]
    public int MaximumSizeOfImage { get; set; } = 1500;

    [Required] public SortOrder SortOrder { get; set; } = SortOrder.SizeDescending;

    public bool IncludeDimensions { get; set; }

    public bool CountOnly { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        _ = sb.Append($"{nameof(SearchFolder)}={SearchFolder}");
        _ = sb.Append($"&{nameof(CurrentPage)}={CurrentPage}");
        _ = sb.Append($"&{nameof(ItemsPerPage)}={ItemsPerPage}");
        _ = sb.Append($"&{nameof(SearchType)}={SearchType}");
        _ = sb.Append($"&{nameof(RecursiveSubDirectories)}={RecursiveSubDirectories}");
        _ = sb.Append($"&{nameof(SortOrder)}={SortOrder}");
        _ = sb.Append($"&{nameof(MaximumSizeOfThumbnail)}={MaximumSizeOfThumbnail}");
        _ = sb.Append($"&{nameof(MaximumSizeOfImage)}={MaximumSizeOfImage}");
        _ = sb.Append($"&{nameof(IncludeDimensions)}={IncludeDimensions}");
        _ = sb.Append($"&{nameof(CountOnly)}={CountOnly}");

        return sb.ToString();
    }
}

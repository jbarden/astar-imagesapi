using AStar.ImagesAPI.Models;

namespace AStar.Web.ImagesAPI.Models;

public class DimensionsShould
{
    [Fact]
    public void ContainTheNullableHeightProperty()
    {
        var sut = new Dimensions();

        _ = sut.Height.Should().BeNull();
    }

    [Fact]
    public void ContainTheNullableWidthProperty()
    {
        var sut = new Dimensions();

        _ = sut.Width.Should().BeNull();
    }

    [Fact]
    public void SetTheHeightToSuppliedIntegerValue()
    {
        const int mockHeight = 1;

        var sut = new Dimensions { Height = mockHeight };

        _ = sut.Height.Should().Be(mockHeight);
    }

    [Fact]
    public void NotSetTheHeightToSuppliedNegativeIntegerValue()
    {
        const int mockHeight = -1;

        Action sut = () => new Dimensions { Height = mockHeight };

        _ = sut.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Height cannot be null or negative. (Parameter 'Height')");
    }

    [Fact]
    public void NotSetTheHeightToSuppliedNullValue()
    {
        Action sut = () => new Dimensions { Height = null };

        _ = sut.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Height cannot be null or negative. (Parameter 'Height')");
    }

    [Fact]
    public void NotSetTheHeightToSuppliedIntegerValueGreaterThanAllowed()
    {
        const int mockHeight = 100_000;

        Action sut = () => new Dimensions { Height = mockHeight };

        _ = sut.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Height cannot be greater than 99999. Actual: 100000. (Parameter 'Height')");
    }

    [Fact]
    public void SetTheWidthToSuppliedIntegerValue()
    {
        const int mockWidth = 1;

        var sut = new Dimensions { Width = mockWidth };

        _ = sut.Width.Should().Be(mockWidth);
    }

    [Fact]
    public void NotSetTheWidthToSuppliedNegativeIntegerValue()
    {
        const int mockWidth = -1;

        Action sut = () => new Dimensions { Width = mockWidth };

        _ = sut.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Width cannot be null or negative. (Parameter 'Width')");
    }

    [Fact]
    public void NotSetTheWidthToSuppliedNullValue()
    {
        Action sut = () => new Dimensions { Width = null };

        _ = sut.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Width cannot be null or negative. (Parameter 'Width')");
    }

    [Fact]
    public void NotSetTheWidthToSuppliedIntegerValueGreaterThanAllowed()
    {
        const int mockWidth = 100_000;

        Action sut = () => new Dimensions { Width = mockWidth };

        _ = sut.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Width cannot be greater than 99999. Actual: 100000. (Parameter 'Width')");
    }
}

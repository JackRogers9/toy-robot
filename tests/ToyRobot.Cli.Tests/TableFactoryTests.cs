using ToyRobot.Cli.Configuration;
using ToyRobot.Cli.Tests.Helpers;
using ToyRobot.Domain;

namespace ToyRobot.Cli.Tests;

public class TableFactoryTests
{
    private readonly FakeRobotOutput _output = new();

    [Fact]
    public void Create_RectangularShape_BuildsTableWithConfiguredBounds()
    {
        var options = new TableOptions { Shape = "Rectangular", Width = 3, Height = 7 };

        var table = TableFactory.Create(options, _output);

        Assert.True(table.CanOccupy(new Position(2, 6)));   // top-right of 3x7
        Assert.False(table.CanOccupy(new Position(3, 0)));  // x past width
        Assert.False(table.CanOccupy(new Position(0, 7)));  // y past height
        Assert.Empty(_output.Messages);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("rectangular")]
    public void Create_AbsentOrRecognisedShape_DoesNotWarn(string? shape)
    {
        var options = new TableOptions { Shape = shape, Width = 5, Height = 5 };

        TableFactory.Create(options, _output);

        Assert.Empty(_output.Messages);
    }

    [Fact]
    public void Create_UnknownShape_FallsBackToRectangularAndWarns()
    {
        var options = new TableOptions { Shape = "Hexagonal", Width = 5, Height = 5 };

        var table = TableFactory.Create(options, _output);

        // Still a usable 5x5 rectangular table.
        Assert.True(table.CanOccupy(new Position(4, 4)));
        Assert.False(table.CanOccupy(new Position(5, 5)));

        var warning = Assert.Single(_output.Messages);
        Assert.Contains("Hexagonal", warning);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 5)]
    [InlineData(5, 0)]
    public void Create_InvalidSize_FallsBackToDefaultAndWarns(int width, int height)
    {
        var options = new TableOptions { Shape = "Rectangular", Width = width, Height = height };

        var table = TableFactory.Create(options, _output);

        // Fell back to the default 5x5.
        Assert.True(table.CanOccupy(new Position(4, 4)));
        Assert.False(table.CanOccupy(new Position(5, 5)));

        var warning = Assert.Single(_output.Messages);
        Assert.Contains("invalid table size", warning);
    }

    [Fact]
    public void Create_InvalidSizeAndUnknownShape_WarnsForBoth()
    {
        var options = new TableOptions { Shape = "R", Width = 0, Height = 0 };

        var table = TableFactory.Create(options, _output);

        Assert.True(table.CanOccupy(new Position(4, 4)));   // default 5x5
        Assert.Equal(2, _output.Messages.Count);
        Assert.Contains(_output.Messages, m => m.Contains("invalid table size"));
        Assert.Contains(_output.Messages, m => m.Contains("'R'"));
    }
}

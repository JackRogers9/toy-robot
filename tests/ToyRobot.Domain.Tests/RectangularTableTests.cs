namespace ToyRobot.Domain.Tests;

public class RectangularTableTests
{
    private readonly RectangularTable _table = new(5, 5);

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(0, 4)]
    [InlineData(4, 0)]
    [InlineData(4, 4)]
    public void CanOccupy_WhenPositionIsOnTable_ReturnsTrue(int x, int y)
    {
        var position = new Position(x, y);

        var result = _table.CanOccupy(position);

        Assert.True(result);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(5, 5)]
    public void CanOccupy_WhenPositionIsOffTable_ReturnsFalse(int x, int y)
    {
        var position = new Position(x, y);

        var result = _table.CanOccupy(position);

        Assert.False(result);
    }

    [Theory]
    // Width 3 (x: 0..2), Height 5 (y: 0..4) — asymmetric to catch axis transposition.
    [InlineData(2, 4, true)]   // top-right corner, valid
    [InlineData(0, 4, true)]   // tallest valid y
    [InlineData(3, 0, false)]  // x past width
    [InlineData(0, 5, false)]  // y past height
    [InlineData(2, 5, false)]  // valid x, invalid y
    public void CanOccupy_RespectsSeparateWidthAndHeight(int x, int y, bool expected)
    {
        var table = new RectangularTable(width: 3, height: 5);

        var result = table.CanOccupy(new Position(x, y));

        Assert.Equal(expected, result);
    }
}

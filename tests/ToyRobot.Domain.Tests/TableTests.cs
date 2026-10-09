namespace ToyRobot.Domain.Tests;

public class TableTests
{
    private readonly Table _table = new(5, 5);

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(0, 4)]
    [InlineData(4, 0)]
    [InlineData(4, 4)]
    public void IsWithinBounds_WhenPositionIsValid_ReturnsTrue(int x, int y)
    {
        var position = new Position(x, y);
        
        var result = _table.IsWithinBounds(position);
        
        Assert.True(result);
    }
    
    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(5, 5)]
    public void IsWithinBounds_WhenPositionIsInvalid_ReturnsFalse(int x, int y)
    {
        var position = new Position(x, y);
        
        var result = _table.IsWithinBounds(position);
        
        Assert.False(result);
    }
}

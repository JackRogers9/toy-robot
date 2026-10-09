namespace ToyRobot.Domain.Tests;

public class RobotTests
{
    private static readonly Table Table = new(5, 5);
    private readonly Robot _robot = new(Table);

    [Theory]
    [InlineData(0, 0, Direction.North)]
    [InlineData(4, 4, Direction.East)]
    [InlineData(2, 3, Direction.South)]
    [InlineData(0, 1, Direction.West)]
    public void TrySetPosition_WithValidPosition_Succeeds(
        int x,
        int y,
        Direction direction
    )
    {
        var position = new Position(x, y);
        
        var result = _robot.TrySetPosition(position, direction);
        
        Assert.True(result);
        Assert.True(_robot.HasPosition);
        Assert.Equal(position, _robot.Position);
        Assert.Equal(direction, _robot.Direction);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(5, 0)]
    [InlineData(0, 5)]
    public void TrySetPosition_WithInvalidPosition_Fails(int x, int y)
    {
        var position = new Position(x, y);
        
        var result = _robot.TrySetPosition(position, Direction.South);
        
        Assert.False(result);
        Assert.False(_robot.HasPosition);
        Assert.Null(_robot.Position);
        Assert.Null(_robot.Direction);
    }
    
    [Fact]
    public void TrySetPosition_WithValidPosition_ReplacesExistingPosition()
    {
        var position = new Position(1, 1);
        var result = _robot.TrySetPosition(position, Direction.North);
        
        Assert.True(result);
        
        var newPosition = new Position(3, 3);
        var updated =  _robot.TrySetPosition(newPosition, Direction.South);
        
        Assert.True(updated);
        Assert.True(_robot.HasPosition);
        Assert.Equal(newPosition, _robot.Position);
        Assert.Equal(Direction.South, _robot.Direction);
    }
    
    [Fact]
    public void Robot_IsNotPlaced_WhenInitialized()
    {
        Assert.False(_robot.HasPosition);
        Assert.Null(_robot.Position);
        Assert.Null(_robot.Direction);
    }
}

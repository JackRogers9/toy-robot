namespace ToyRobot.Domain.Tests;

public class RobotTests
{
    private static readonly RectangularTable Table = new(5, 5);
    private readonly Robot _robot = new(Table);

    [Theory]
    [InlineData(0, 0, Direction.North)]
    [InlineData(4, 4, Direction.East)]
    [InlineData(2, 3, Direction.South)]
    [InlineData(0, 1, Direction.West)]
    public void TryPlace_WithValidPosition_Succeeds(
        int x,
        int y,
        Direction direction
    )
    {
        var position = new Position(x, y);
        
        var result = _robot.TryPlace(position, direction);
        
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
    public void TryPlace_WithInvalidPosition_Fails(int x, int y)
    {
        var position = new Position(x, y);
        
        var result = _robot.TryPlace(position, Direction.South);
        
        Assert.False(result);
        Assert.False(_robot.HasPosition);
        Assert.Null(_robot.Position);
        Assert.Null(_robot.Direction);
    }
    
    [Fact]
    public void TryPlace_WithValidPosition_ReplacesExistingPosition()
    {
        _robot.TryPlace(new Position(1, 1), Direction.North);
        
        var position = new Position(3, 3);
        var updated =  _robot.TryPlace(position, Direction.South);
        
        Assert.True(updated);
        Assert.True(_robot.HasPosition);
        Assert.Equal(position, _robot.Position);
        Assert.Equal(Direction.South, _robot.Direction);
    }
    
    [Fact]
    public void Robot_IsNotPlaced_WhenInitialized()
    {
        Assert.False(_robot.HasPosition);
        Assert.Null(_robot.Position);
        Assert.Null(_robot.Direction);
    }

    [Theory]
    [InlineData(Direction.North, 2, 3)]
    [InlineData(Direction.East, 3, 2)]
    [InlineData(Direction.South, 2, 1)]
    [InlineData(Direction.West, 1, 2)]
    public void TryMove_WithinBounds_MovesOneSquare(
        Direction direction,
        int expectedX,
        int expectedY
    )
    {
        _robot.TryPlace(new Position(2, 2), direction);

        var result = _robot.TryMove();

        Assert.True(result);
        Assert.Equal(new Position(expectedX, expectedY), _robot.Position);
        Assert.Equal(direction, _robot.Direction);
    }

    [Theory]
    [InlineData(0, 4, Direction.North)]
    [InlineData(4, 0, Direction.East)]
    [InlineData(0, 0, Direction.South)]
    [InlineData(0, 0, Direction.West)]
    public void TryMove_OutOfBounds_DoesNotMove(int x, int y, Direction direction)
    {
        var position = new Position(x, y);
        _robot.TryPlace(position, direction);

        var result = _robot.TryMove();

        Assert.False(result);
        Assert.Equal(position, _robot.Position);
        Assert.Equal(direction, _robot.Direction);
    }

    [Fact]
    public void TryMove_WhenNotPlaced_Fails()
    {
        var result = _robot.TryMove();

        Assert.False(result);
        Assert.False(_robot.HasPosition);
        Assert.Null(_robot.Position);
        Assert.Null(_robot.Direction);
    }

    [Theory]
    [InlineData(Direction.North, Direction.West)]
    [InlineData(Direction.West, Direction.South)]
    [InlineData(Direction.South, Direction.East)]
    [InlineData(Direction.East, Direction.North)]
    public void TryTurnLeft_WhenPlaced_RotatesNinetyDegrees(Direction facing, Direction expected)
    {
        var position = new Position(2, 2);
        _robot.TryPlace(position, facing);

        var result = _robot.TryTurnLeft();

        Assert.True(result);
        Assert.Equal(expected, _robot.Direction);
        Assert.Equal(position, _robot.Position);
    }

    [Theory]
    [InlineData(Direction.North, Direction.East)]
    [InlineData(Direction.East, Direction.South)]
    [InlineData(Direction.South, Direction.West)]
    [InlineData(Direction.West, Direction.North)]
    public void TryTurnRight_WhenPlaced_RotatesNinetyDegrees(Direction facing, Direction expected)
    {
        var position = new Position(2, 2);
        _robot.TryPlace(position, facing);

        var result = _robot.TryTurnRight();

        Assert.True(result);
        Assert.Equal(expected, _robot.Direction);
        Assert.Equal(position, _robot.Position);
    }

    [Fact]
    public void TryTurnLeft_WhenNotPlaced_Fails()
    {
        var result = _robot.TryTurnLeft();

        Assert.False(result);
        Assert.Null(_robot.Direction);
    }

    [Fact]
    public void TryTurnRight_WhenNotPlaced_Fails()
    {
        var result = _robot.TryTurnRight();

        Assert.False(result);
        Assert.Null(_robot.Direction);
    }
}

namespace ToyRobot.Domain.Tests;

public class DirectionExtensionsTests
{
    [Theory]
    [InlineData(Direction.North, Direction.West)]
    [InlineData(Direction.West, Direction.South)]
    [InlineData(Direction.South, Direction.East)]
    [InlineData(Direction.East, Direction.North)]
    public void TurnLeft_RotatesNinetyDegreesAnticlockwise(Direction facing, Direction expected)
    {
        var result = facing.TurnLeft();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Direction.North, Direction.East)]
    [InlineData(Direction.East, Direction.South)]
    [InlineData(Direction.South, Direction.West)]
    [InlineData(Direction.West, Direction.North)]
    public void TurnRight_RotatesNinetyDegreesClockwise(Direction facing, Direction expected)
    {
        var result = facing.TurnRight();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Direction.North)]
    [InlineData(Direction.East)]
    [InlineData(Direction.South)]
    [InlineData(Direction.West)]
    public void TurnLeft_FourTimes_ReturnsToStart(Direction facing)
    {
        var result = facing.TurnLeft().TurnLeft().TurnLeft().TurnLeft();

        Assert.Equal(facing, result);
    }

    [Theory]
    [InlineData(Direction.North)]
    [InlineData(Direction.East)]
    [InlineData(Direction.South)]
    [InlineData(Direction.West)]
    public void TurnRight_FourTimes_ReturnsToStart(Direction facing)
    {
        var result = facing.TurnRight().TurnRight().TurnRight().TurnRight();

        Assert.Equal(facing, result);
    }

    [Theory]
    [InlineData(Direction.North)]
    [InlineData(Direction.East)]
    [InlineData(Direction.South)]
    [InlineData(Direction.West)]
    public void TurnLeft_ThenTurnRight_ReturnsToStart(Direction facing)
    {
        var result = facing.TurnLeft().TurnRight();

        Assert.Equal(facing, result);
    }
}

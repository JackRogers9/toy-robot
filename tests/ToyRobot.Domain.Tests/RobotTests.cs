using ToyRobot.Domain.Tests.Helpers;

namespace ToyRobot.Domain.Tests;

public class RobotTests
{
    [Theory]
    [InlineData(0, 0, Direction.North)]
    [InlineData(4, 4, Direction.East)]
    [InlineData(2, 3, Direction.South)]
    public void TryPlace_WhenTableAllowsPosition_Succeeds(int x, int y, Direction direction)
    {
        var robot = new Robot(FakeTable.AllowingEverything());
        var position = new Position(x, y);

        var result = robot.TryPlace(position, direction);

        Assert.True(result);
        Assert.True(robot.HasPosition);
        Assert.Equal(position, robot.Position);
        Assert.Equal(direction, robot.Direction);
    }

    [Fact]
    public void TryPlace_WhenTableBlocksPosition_Fails()
    {
        var robot = new Robot(FakeTable.AllowingNothing());

        var result = robot.TryPlace(new Position(1, 1), Direction.North);

        Assert.False(result);
        Assert.False(robot.HasPosition);
        Assert.Null(robot.Position);
        Assert.Null(robot.Direction);
    }

    [Fact]
    public void TryPlace_WhenAlreadyPlaced_ReplacesPosition()
    {
        var robot = new Robot(FakeTable.AllowingEverything());
        robot.TryPlace(new Position(1, 1), Direction.North);

        var newPosition = new Position(3, 3);
        var result = robot.TryPlace(newPosition, Direction.South);

        Assert.True(result);
        Assert.Equal(newPosition, robot.Position);
        Assert.Equal(Direction.South, robot.Direction);
    }

    [Fact]
    public void Robot_IsNotPlaced_WhenInitialized()
    {
        var robot = new Robot(FakeTable.AllowingEverything());

        Assert.False(robot.HasPosition);
        Assert.Null(robot.Position);
        Assert.Null(robot.Direction);
    }

    [Theory]
    [InlineData(Direction.North, 2, 3)]
    [InlineData(Direction.East, 3, 2)]
    [InlineData(Direction.South, 2, 1)]
    [InlineData(Direction.West, 1, 2)]
    public void TryMove_WhenTargetCellIsAllowed_MovesOneSquare(
        Direction direction,
        int expectedX,
        int expectedY
    )
    {
        var robot = new Robot(FakeTable.AllowingEverything());
        robot.TryPlace(new Position(2, 2), direction);

        var result = robot.TryMove();

        Assert.True(result);
        Assert.Equal(new Position(expectedX, expectedY), robot.Position);
        Assert.Equal(direction, robot.Direction);
    }

    [Fact]
    public void TryMove_WhenTargetCellIsBlocked_DoesNotMove()
    {
        var start = new Position(2, 2);
        // Only the starting cell is allowed, so any move off it is blocked.
        var robot = new Robot(FakeTable.Allowing(start));
        robot.TryPlace(start, Direction.North);

        var result = robot.TryMove();

        Assert.False(result);
        Assert.Equal(start, robot.Position);
        Assert.Equal(Direction.North, robot.Direction);
    }

    [Fact]
    public void TryMove_WhenNotPlaced_Fails()
    {
        var robot = new Robot(FakeTable.AllowingEverything());

        var result = robot.TryMove();

        Assert.False(result);
        Assert.False(robot.HasPosition);
    }

    [Theory]
    [InlineData(Direction.North, Direction.West)]
    [InlineData(Direction.West, Direction.South)]
    [InlineData(Direction.South, Direction.East)]
    [InlineData(Direction.East, Direction.North)]
    public void TryTurnLeft_WhenPlaced_RotatesNinetyDegrees(Direction facing, Direction expected)
    {
        var robot = new Robot(FakeTable.AllowingEverything());
        var position = new Position(2, 2);
        robot.TryPlace(position, facing);

        var result = robot.TryTurnLeft();

        Assert.True(result);
        Assert.Equal(expected, robot.Direction);
        Assert.Equal(position, robot.Position);
    }

    [Theory]
    [InlineData(Direction.North, Direction.East)]
    [InlineData(Direction.East, Direction.South)]
    [InlineData(Direction.South, Direction.West)]
    [InlineData(Direction.West, Direction.North)]
    public void TryTurnRight_WhenPlaced_RotatesNinetyDegrees(Direction facing, Direction expected)
    {
        var robot = new Robot(FakeTable.AllowingEverything());
        var position = new Position(2, 2);
        robot.TryPlace(position, facing);

        var result = robot.TryTurnRight();

        Assert.True(result);
        Assert.Equal(expected, robot.Direction);
        Assert.Equal(position, robot.Position);
    }

    [Fact]
    public void TryTurnLeft_WhenNotPlaced_Fails()
    {
        var robot = new Robot(FakeTable.AllowingEverything());

        var result = robot.TryTurnLeft();

        Assert.False(result);
        Assert.Null(robot.Direction);
    }

    [Fact]
    public void TryTurnRight_WhenNotPlaced_Fails()
    {
        var robot = new Robot(FakeTable.AllowingEverything());

        var result = robot.TryTurnRight();

        Assert.False(result);
        Assert.Null(robot.Direction);
    }
}

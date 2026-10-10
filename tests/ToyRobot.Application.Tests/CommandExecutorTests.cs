using ToyRobot.Application.Commands;
using ToyRobot.Application.Handlers;
using ToyRobot.Application.Interfaces;
using ToyRobot.Domain;

namespace ToyRobot.Application.Tests;

public class CommandExecutorTests
{
    private readonly Robot _robot = new(new Table(5, 5));
    private readonly FakeRobotOutput _output = new();
    private readonly CommandExecutor _executor;

    public CommandExecutorTests()
    {
        _executor = new CommandExecutor(_robot, _output);
    }

    [Fact]
    public void Execute_PlaceCommand_PlacesRobot()
    {
        _executor.Execute(new PlaceCommand(new Position(1, 2), Direction.North));

        Assert.True(_robot.HasPosition);
        Assert.Equal(new Position(1, 2), _robot.Position);
        Assert.Equal(Direction.North, _robot.Direction);
    }

    [Fact]
    public void Execute_MoveCommand_MovesRobotForward()
    {
        _executor.Execute(new PlaceCommand(new Position(2, 2), Direction.North));

        _executor.Execute(new MoveCommand());

        Assert.Equal(new Position(2, 3), _robot.Position);
    }

    [Fact]
    public void Execute_LeftCommand_TurnsRobotLeft()
    {
        _executor.Execute(new PlaceCommand(new Position(2, 2), Direction.North));

        _executor.Execute(new LeftCommand());

        Assert.Equal(Direction.West, _robot.Direction);
    }

    [Fact]
    public void Execute_RightCommand_TurnsRobotRight()
    {
        _executor.Execute(new PlaceCommand(new Position(2, 2), Direction.North));

        _executor.Execute(new RightCommand());

        Assert.Equal(Direction.East, _robot.Direction);
    }

    [Fact]
    public void Execute_ReportCommand_WritesFormattedPositionAndDirection()
    {
        _executor.Execute(new PlaceCommand(new Position(1, 2), Direction.East));

        _executor.Execute(new ReportCommand());

        Assert.Equal("1,2,EAST", Assert.Single(_output.Messages));
    }

    [Fact]
    public void Execute_ReportCommand_BeforePlace_WritesNothing()
    {
        _executor.Execute(new ReportCommand());

        Assert.Empty(_output.Messages);
    }

    [Theory]
    [InlineData(typeof(MoveCommand))]
    [InlineData(typeof(LeftCommand))]
    [InlineData(typeof(RightCommand))]
    public void Execute_CommandBeforePlace_IsIgnored(Type commandType)
    {
        var command = (IRobotCommand)Activator.CreateInstance(commandType)!;

        _executor.Execute(command);

        Assert.False(_robot.HasPosition);
    }

    [Fact]
    public void Execute_InvalidPlace_LeavesRobotUnplaced()
    {
        _executor.Execute(new PlaceCommand(new Position(9, 9), Direction.North));

        Assert.False(_robot.HasPosition);
    }

    [Fact]
    public void Execute_CommandsAfterFailedPlace_StayIgnored()
    {
        _executor.Execute(new PlaceCommand(new Position(9, 9), Direction.North));

        _executor.Execute(new MoveCommand());
        _executor.Execute(new ReportCommand());

        Assert.False(_robot.HasPosition);
        Assert.Empty(_output.Messages);
    }
}

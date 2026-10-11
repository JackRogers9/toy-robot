using ToyRobot.Application.Handlers;
using ToyRobot.Application.Tests.Helpers;
using ToyRobot.Domain;

namespace ToyRobot.Application.Tests;

public class RobotConsoleTests
{
    private readonly FakeRobotOutput _output = new();

    private RobotConsole BuildConsole(params string[] lines)
    {
        var robot = new Robot(new RectangularTable(5, 5));
        var executor = new CommandExecutor(robot, _output);
        
        return new RobotConsole(
            new ScriptedRobotInput(lines),
            _output, 
            new CommandParser(), 
            executor
        );
    }

    [Fact]
    public void Run_WritesOpeningMessageFirstAndGoodbyeLast()
    {
        BuildConsole().Run();

        Assert.StartsWith("Toy Robot!", _output.Messages[0]);
        Assert.Equal("Goodbye!", _output.Messages[^1]);
    }

    [Fact]
    public void Run_ExecutesCommandsAndReportsOutput()
    {
        BuildConsole("PLACE 1,2,NORTH", "MOVE", "REPORT").Run();

        Assert.Contains("1,3,NORTH", _output.Messages);
    }

    [Fact]
    public void Run_IsCaseInsensitive()
    {
        BuildConsole("place 0,0,east", "report").Run();

        Assert.Contains("0,0,EAST", _output.Messages);
    }

    [Fact]
    public void Run_StopsAtEndOfInput_AndSaysGoodbye()
    {
        BuildConsole("PLACE 1,1,NORTH", "REPORT").Run();

        Assert.Equal("Goodbye!", _output.Messages[^1]);
    }

    [Fact]
    public void Run_IgnoresInvalidInput()
    {
        BuildConsole("GARBAGE", "PLACE 2,2,WEST", "REPORT").Run();

        Assert.Contains("2,2,WEST", _output.Messages);
    }

    [Fact]
    public void Run_IgnoresCommandsBeforePlace()
    {
        BuildConsole("MOVE", "REPORT").Run();
        
        Assert.Equal(2, _output.Messages.Count);
    }
}

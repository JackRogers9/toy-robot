using ToyRobot.Application.Commands;
using ToyRobot.Application.Handlers;
using ToyRobot.Domain;

namespace ToyRobot.Application.Tests;

public class CommandParserTests
{
    private readonly CommandParser _parser = new();

    [Theory]
    [InlineData("MOVE")]
    [InlineData("move")]
    [InlineData("  MOVE  ")]
    public void Parse_Move_ReturnsMoveCommand(string input)
    {
        var result = _parser.Parse(input);

        Assert.IsType<MoveCommand>(result);
    }

    [Theory]
    [InlineData("LEFT")]
    [InlineData("left")]
    public void Parse_Left_ReturnsLeftCommand(string input)
    {
        var result = _parser.Parse(input);

        Assert.IsType<LeftCommand>(result);
    }

    [Theory]
    [InlineData("RIGHT")]
    [InlineData("right")]
    public void Parse_Right_ReturnsRightCommand(string input)
    {
        var result = _parser.Parse(input);

        Assert.IsType<RightCommand>(result);
    }

    [Theory]
    [InlineData("REPORT")]
    [InlineData("report")]
    public void Parse_Report_ReturnsReportCommand(string input)
    {
        var result = _parser.Parse(input);

        Assert.IsType<ReportCommand>(result);
    }

    [Theory]
    [InlineData("PLACE 1,2,NORTH", 1, 2, Direction.North)]
    [InlineData("place 0,0,east", 0, 0, Direction.East)]
    [InlineData("PLACE 3, 4 , South", 3, 4, Direction.South)]
    [InlineData("PLACE  2,1,WEST", 2, 1, Direction.West)]
    public void Parse_ValidPlace_ReturnsPlaceCommandWithValues(
        string input,
        int expectedX,
        int expectedY,
        Direction expectedDirection
    )
    {
        var result = _parser.Parse(input);

        var place = Assert.IsType<PlaceCommand>(result);
        Assert.Equal(new Position(expectedX, expectedY), place.Position);
        Assert.Equal(expectedDirection, place.Direction);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_NullOrWhitespace_ReturnsNull(string? input)
    {
        var result = _parser.Parse(input);

        Assert.Null(result);
    }

    [Theory]
    [InlineData("WRONG")]
    [InlineData("PLACEE 1,2,NORTH")]
    [InlineData("")]
    public void Parse_UnknownKeyword_ReturnsNull(string input)
    {
        var result = _parser.Parse(input);

        Assert.Null(result);
    }

    [Theory]
    [InlineData("MOVE EXTRA")]
    [InlineData("LEFT 1")]
    [InlineData("REPORT NOW")]
    public void Parse_CommandWithArguments_ReturnsNull(string input)
    {
        var result = _parser.Parse(input);

        Assert.Null(result);
    }

    [Theory]
    [InlineData("PLACE 1,2")]
    [InlineData("PLACE 1,2,NORTH,EXTRA")]
    [InlineData("PLACE 1,2,UP")]
    [InlineData("PLACE x,2,NORTH")]
    [InlineData("PLACE 1,y,NORTH")]
    [InlineData("PLACE")]
    public void Parse_InvalidPlaceArguments_ReturnsNull(string input)
    {
        var result = _parser.Parse(input);

        Assert.Null(result);
    }
    
    [Theory]
    [InlineData("EXIT")]
    [InlineData("exit")]
    [InlineData("  Exit  ")]
    public void Parse_Exit_ReturnsExitCommand(string input)
    {
        var result = _parser.Parse(input);

        Assert.IsType<ExitCommand>(result);
    }
}

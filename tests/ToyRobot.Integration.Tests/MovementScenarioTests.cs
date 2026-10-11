using ToyRobot.Integration.Tests.Helpers;

namespace ToyRobot.Integration.Tests;

/// <summary>
/// End-to-end movement scenarios asserting on the CLI's REPORT output.
/// </summary>
public class MovementScenarioTests(ITestOutputHelper output) : IntegrationTestBase(output)
{
    [Fact]
    public async Task Place_Move_Report_AdvancesOneSquare()
    {
        var reports = await GetReportsAsync("PLACE 0,0,NORTH", "MOVE", "REPORT");

        Assert.Equal("0,1,NORTH", Assert.Single(reports));
    }

    [Fact]
    public async Task Place_Left_Report_TurnsAnticlockwise()
    {
        var reports = await GetReportsAsync("PLACE 0,0,NORTH", "LEFT", "REPORT");

        Assert.Equal("0,0,WEST", Assert.Single(reports));
    }

    [Fact]
    public async Task CombinedSequence_ProducesExpectedFinalPosition()
    {
        var reports = await GetReportsAsync(
            "PLACE 1,2,EAST", "MOVE", "MOVE", "LEFT", "MOVE", "REPORT");

        Assert.Equal("3,3,NORTH", Assert.Single(reports));
    }

    [Fact]
    public async Task CommandsBeforePlace_AreIgnored()
    {
        var reports = await GetReportsAsync(
            "MOVE", "LEFT", "REPORT", "PLACE 2,2,SOUTH", "REPORT");

        // The REPORT before PLACE must produce nothing; only the post-place one does.
        Assert.Equal("2,2,SOUTH", Assert.Single(reports));
    }

    [Fact]
    public async Task OffTableMove_IsIgnored()
    {
        var reports = await GetReportsAsync("PLACE 4,4,NORTH", "MOVE", "REPORT");

        // Facing north at the top edge of a 5x5 board: MOVE is rejected.
        Assert.Equal("4,4,NORTH", Assert.Single(reports));
    }

    [Fact]
    public async Task InvalidInput_IsIgnored()
    {
        var reports = await GetReportsAsync(
            "GARBAGE", "PLACE 1,1,EAST", "MOVE EXTRA", "REPORT");

        // GARBAGE and the bare command with trailing args are ignored;
        // the robot stays where PLACE put it.
        Assert.Equal("1,1,EAST", Assert.Single(reports));
    }
}

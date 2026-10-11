using ToyRobot.Cli.Configuration;
using ToyRobot.Integration.Tests.Helpers;

namespace ToyRobot.Integration.Tests;

/// <summary>
/// Verifies the table size is driven by configuration end to end, overriding
/// the board at launch and asserting the running app honours the new bounds.
/// </summary>
public class ConfigurationScenarioTests(ITestOutputHelper output) : IntegrationTestBase(output)
{
    [Fact]
    public async Task LargerBoard_AcceptsPositionsBeyondTheDefaultFiveByFive()
    {
        // (5,5) and a move to (5,6) are off a 5x5 board but valid on 7x7.
        var table = new TableOptions
        {
            Width = 7, 
            Height = 7
        };

        var reports = await GetReportsAsync(table, "PLACE 5,5,NORTH", "MOVE", "REPORT", "EXIT");

        Assert.Equal("5,6,NORTH", Assert.Single(reports));
    }

    [Fact]
    public async Task SmallerBoard_RejectsPositionsValidOnTheDefaultFiveByFive()
    {
        // (4,4) is the top-right corner of a 5x5 board but off a 3x3 board.
        var table = new TableOptions
        {
            Width = 3, 
            Height = 3
        };

        var reports = await GetReportsAsync(table, "PLACE 4,4,NORTH", "REPORT", "EXIT");

        Assert.Empty(reports);
    }

    [Fact]
    public async Task SmallerBoard_BlocksMovesAtItsOwnEdge()
    {
        // On a 3x3 board the top edge is y=2, so a northward move is rejected.
        var table = new TableOptions
        {
            Width = 3, 
            Height = 3
        };

        var reports = await GetReportsAsync(table, "PLACE 2,2,NORTH", "MOVE", "REPORT", "EXIT");

        Assert.Equal("2,2,NORTH", Assert.Single(reports));
    }

    [Fact]
    public async Task DefaultBoard_StillRejectsPositionsOffTheFiveByFive()
    {
        // No override: the baked-in appsettings 5x5 applies, so (6,6) is invalid.
        var reports = await GetReportsAsync("PLACE 6,6,NORTH", "REPORT", "EXIT");

        Assert.Empty(reports);
    }
}

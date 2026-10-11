using ToyRobot.Integration.Tests.Helpers;

namespace ToyRobot.Integration.Tests;

/// <summary>
/// Smoke tests confirming the CLI starts and shuts down cleanly.
/// </summary>
public class SmokeTests(ITestOutputHelper output) : IntegrationTestBase(output)
{
    [Fact]
    public async Task Cli_StartsAndPrintsOpeningMessage()
    {
        var result = await RunAsync();

        Assert.Contains("Toy Robot!", result.StandardOutput);
    }

    [Fact]
    public async Task Cli_ExitsCleanlyWithGoodbye()
    {
        var result = await RunAsync();

        Assert.Equal(0, result.ExitCode);
        Assert.Contains("Goodbye!", result.StandardOutput);
    }
}

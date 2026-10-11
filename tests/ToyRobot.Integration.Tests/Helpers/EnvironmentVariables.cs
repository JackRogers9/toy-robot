using ToyRobot.Cli.Configuration;

namespace ToyRobot.Integration.Tests.Helpers;

/// <summary>
/// Builds the environment variables that override the CLI's table configuration.
/// </summary>
public static class EnvironmentVariables
{
    public static IReadOnlyDictionary<string, string> From(TableOptions table) =>
        new Dictionary<string, string>
        {
            ["Table__Width"] = table.Width.ToString(),
            ["Table__Height"] = table.Height.ToString(),
        };
}

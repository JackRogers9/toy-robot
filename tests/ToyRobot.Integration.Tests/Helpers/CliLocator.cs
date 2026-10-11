using ToyRobot.Cli;

namespace ToyRobot.Integration.Tests.Helpers;

/// <summary>
/// Resolves the path to the built ToyRobot.Cli DLL via the referenced assembly.
/// </summary>
public static class CliLocator
{
    public static string DllPath => typeof(ConsoleRobotInput).Assembly.Location;
}

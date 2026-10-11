namespace ToyRobot.Integration.Tests.Helpers;

/// <summary>
/// Extracts the robot's REPORT lines from the CLI's stdout.
/// </summary>
public static class ReportOutput
{
    private const string OpeningMessage = "Toy Robot!";
    private const string Goodbye = "Goodbye!";

    public static IReadOnlyList<string> Parse(string standardOutput) =>
    [
        .. standardOutput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(IsReportLine)
    ];

    private static bool IsReportLine(string line) =>
        !line.StartsWith(OpeningMessage, StringComparison.Ordinal) &&
        !line.Equals(Goodbye, StringComparison.Ordinal);
}

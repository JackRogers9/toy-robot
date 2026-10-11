using ToyRobot.Cli.Configuration;

namespace ToyRobot.Integration.Tests.Helpers;

/// <summary>
/// Base class for integration tests: runs the CLI through the extracted
/// helpers and logs its output so failed assertions are easy to diagnose.
/// </summary>
[Trait("Category", "Integration")]
public abstract class IntegrationTestBase(ITestOutputHelper output)
{
    protected Task<CliResult> RunAsync(params string[] commands) => RunAsync(environment: null, commands);

    protected async Task<IReadOnlyList<string>> GetReportsAsync(params string[] commands)
    {
        var result = await RunAsync(commands);
        
        return ReportOutput.Parse(result.StandardOutput);
    }

    /// <summary>Runs the CLI with the table size overridden via configuration.</summary>
    protected async Task<IReadOnlyList<string>> GetReportsAsync(
        TableOptions table,
        params string[] commands
    )
    {
        var environmentVariables = EnvironmentVariables.From(table);
        var result = await RunAsync(environmentVariables, commands);
        
        return ReportOutput.Parse(result.StandardOutput);
    }

    private async Task<CliResult> RunAsync(
        IReadOnlyDictionary<string, string>? environment,
        params string[] commands
    )
    {
        var result = await CliProcessRunner.RunAsync(CliLocator.DllPath, commands, environment);
        Log(result);
        return result;
    }

    private void Log(CliResult result)
    {
        output.WriteLine("--- toy-robot stdout (exit {0}) ---", result.ExitCode);

        if (!string.IsNullOrWhiteSpace(result.StandardOutput))
        {
            output.WriteLine(result.StandardOutput);
        }

        if (!string.IsNullOrWhiteSpace(result.StandardError))
        {
            output.WriteLine("STDERR:\n{0}", result.StandardError);
        }
    }
}

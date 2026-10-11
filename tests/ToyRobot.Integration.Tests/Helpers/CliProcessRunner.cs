using System.Diagnostics;

namespace ToyRobot.Integration.Tests.Helpers;

/// <summary>
/// The outcome of a CLI run: exit code and captured streams.
/// </summary>
public sealed record CliResult(int ExitCode, string StandardOutput, string StandardError);

/// <summary>
/// Runs the CLI as a child process, reading stdout/stderr concurrently to avoid
/// stream-buffer deadlocks. Knows nothing about xUnit or report semantics.
/// </summary>
public static class CliProcessRunner
{
    public static async Task<CliResult> RunAsync(
        string cliDllPath,
        IEnumerable<string> commands,
        IReadOnlyDictionary<string, string>? environment = null,
        CancellationToken cancellationToken = default
    )
    {
        using var process = Process.Start(BuildStartInfo(cliDllPath, environment))
            ?? throw new InvalidOperationException("Failed to start the ToyRobot.Cli process.");

        var inputTask = PipeCommandsAsync(process, commands, cancellationToken);
        var stdoutTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stderrTask = process.StandardError.ReadToEndAsync(cancellationToken);

        await Task.WhenAll(inputTask, stdoutTask, stderrTask);
        await process.WaitForExitAsync(cancellationToken);

        return new CliResult(process.ExitCode, await stdoutTask, await stderrTask);
    }

    private static ProcessStartInfo BuildStartInfo(
        string cliDllPath,
        IReadOnlyDictionary<string, string>? environment
    )
    {
        var startInfo = new ProcessStartInfo("dotnet")
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add(cliDllPath);

        if (environment is null)
        {
            return startInfo;
        }
        
        foreach (var (key, value) in environment)
        {
            startInfo.Environment[key] = value;
        }

        return startInfo;
    }

    private static async Task PipeCommandsAsync(
        Process process,
        IEnumerable<string> commands,
        CancellationToken cancellationToken
    )
    {
        foreach (var command in commands)
        {
            await process.StandardInput.WriteLineAsync(command.AsMemory(), cancellationToken);
        }

        await process.StandardInput.FlushAsync(cancellationToken);
        process.StandardInput.Close();
    }
}

namespace ToyRobot.Cli;

/// <summary>
/// Allows Ctrl-C to be handled gracefully
/// </summary>
public sealed class ConsoleCancellationHandler : IDisposable
{
    private readonly CancellationTokenSource _cancellation = new();

    public ConsoleCancellationHandler()
    {
        Console.CancelKeyPress += OnCancelKeyPress;
    }

    public CancellationToken Token => _cancellation.Token;

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs eventArgs)
    {
        eventArgs.Cancel = true;
        _cancellation.Cancel();
    }

    public void Dispose()
    {
        Console.CancelKeyPress -= OnCancelKeyPress;
        _cancellation.Dispose();
    }
}

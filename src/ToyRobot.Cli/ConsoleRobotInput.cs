using ToyRobot.Application.Interfaces;

namespace ToyRobot.Cli;

public class ConsoleRobotInput(CancellationToken cancellationToken) : IRobotInput
{
    public string? ReadLine()
    {
        // Console.ReadLine blocks and cannot be cancelled, so run it on a
        // background task and wait on either the read or cancellation.
        var readTask = Task.Run(Console.ReadLine);

        try
        {
            readTask.Wait(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Ctrl-C: signal end of input. The read task is still blocked but is
            // deliberately abandoned — the process is exiting, so leaking it is fine.
            return null;
        }

        return readTask.Result;
    }
}

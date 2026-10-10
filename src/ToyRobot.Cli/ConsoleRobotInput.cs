using ToyRobot.Application.Interfaces;

namespace ToyRobot.Cli;

public class ConsoleRobotInput(CancellationToken cancellationToken) : IRobotInput
{
    public string? ReadLine()
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        var readTask = Task.Run(Console.ReadLine);

        try
        {
            readTask.Wait(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // Ctrl-C pressed
            return null;
        }

        return readTask.Result;
    }
}

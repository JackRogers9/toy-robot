using ToyRobot.Application.Interfaces;

namespace ToyRobot.Cli;

public class ConsoleRobotOutput : IRobotOutput
{
    public void Write(string message)
    {
        Console.WriteLine(message);
    }
}

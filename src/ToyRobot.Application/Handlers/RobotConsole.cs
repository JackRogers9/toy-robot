using ToyRobot.Application.Commands;
using ToyRobot.Application.Interfaces;

namespace ToyRobot.Application.Handlers;

public class RobotConsole(
    IRobotInput input,
    IRobotOutput output,
    ICommandParser parser,
    ICommandExecutor executor
)
{
    private const string OpeningMessage =
        "Toy Robot! Enter commands: PLACE X,Y,DIRECTION | MOVE | LEFT | RIGHT | REPORT (EXIT to quit)";

    public void Run()
    {
        output.Write(OpeningMessage);

        while (input.ReadLine() is { } line)
        {
            var command = parser.Parse(line);

            if (command is ExitCommand)
            {
                break;
            }

            if (command is not null)
            {
                executor.Execute(command);
            }
        }

        output.Write("Goodbye!");
    }
}

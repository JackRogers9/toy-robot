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
        "Toy Robot! Enter commands: PLACE X,Y,DIRECTION | MOVE | LEFT | RIGHT | REPORT";

    public void Run()
    {
        output.Write(OpeningMessage);

        while (input.ReadLine() is { } line)
        {
            var command = parser.Parse(line);

            if (command is not null)
            {
                executor.Execute(command);
            }
        }

        output.Write("Goodbye!");
    }
}

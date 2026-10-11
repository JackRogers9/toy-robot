using ToyRobot.Application.Commands;
using ToyRobot.Application.Interfaces;
using ToyRobot.Domain;

namespace ToyRobot.Application.Handlers;

public class CommandExecutor(Robot robot, IRobotOutput output) : ICommandExecutor
{
    public void Execute(IRobotCommand command)
    {
        // First command has to be a Place Command
        if (!robot.HasPosition && command is not PlaceCommand)
        {
            return;
        }

        switch (command)
        {
            case PlaceCommand place:
                robot.TryPlace(place.Position, place.Direction);
                break;
            case MoveCommand:
                robot.TryMove();
                break;
            case LeftCommand:
                robot.TryTurnLeft();
                break;
            case RightCommand:
                robot.TryTurnRight();
                break;
            case ReportCommand:
                Report();
                break;
        }
    }

    private void Report()
    {
        if (robot.Position is not { } position || robot.Direction is not { } direction)
        {
            return;
        }

        var facing = direction.ToString().ToUpperInvariant();
        output.Write($"{position.X},{position.Y},{facing}");
    }
}

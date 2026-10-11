using ToyRobot.Application.Commands;
using ToyRobot.Application.Interfaces;
using ToyRobot.Domain;

namespace ToyRobot.Application.Handlers;

public class CommandExecutor(Robot robot, IRobotOutput output) : ICommandExecutor
{
    private readonly RobotContext _context = new(robot, output);

    public void Execute(IRobotCommand command)
    {
        // The robot must be placed before any other command takes effect.
        if (!robot.HasPosition && command is not PlaceCommand)
        {
            return;
        }

        command.Execute(_context);
    }
}

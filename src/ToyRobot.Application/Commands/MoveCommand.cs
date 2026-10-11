using ToyRobot.Application.Interfaces;

namespace ToyRobot.Application.Commands;

public class MoveCommand : IRobotCommand
{
    public void Execute(RobotContext context) => context.Robot.TryMove();
}

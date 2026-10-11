using ToyRobot.Application.Interfaces;

namespace ToyRobot.Application.Commands;

public class RightCommand : IRobotCommand
{
    public void Execute(RobotContext context) => context.Robot.TryTurnRight();
}

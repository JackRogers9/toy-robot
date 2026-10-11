using ToyRobot.Application.Interfaces;

namespace ToyRobot.Application.Commands;

public class LeftCommand : IRobotCommand
{
    public void Execute(RobotContext context) => context.Robot.TryTurnLeft();
}

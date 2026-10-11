using ToyRobot.Application.Commands;

namespace ToyRobot.Application.Interfaces;

public interface IRobotCommand
{
    void Execute(RobotContext context);
}

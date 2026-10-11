namespace ToyRobot.Application.Interfaces;

public interface ICommandExecutor
{
    void Execute(IRobotCommand command);
}

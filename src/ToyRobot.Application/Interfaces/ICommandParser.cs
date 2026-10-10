namespace ToyRobot.Application.Interfaces;

public interface ICommandParser
{
    IRobotCommand? Parse(string? input);
}

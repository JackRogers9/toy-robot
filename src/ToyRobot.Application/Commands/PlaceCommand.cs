using ToyRobot.Application.Interfaces;
using ToyRobot.Domain;

namespace ToyRobot.Application.Commands;

public class PlaceCommand(Position position, Direction direction) : IRobotCommand
{
    public Position Position { get; } = position;
    public Direction Direction { get; } = direction;
}

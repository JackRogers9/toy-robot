using ToyRobot.Domain.Extensions;
using ToyRobot.Domain.Interfaces;

namespace ToyRobot.Domain;

/// <summary>
/// The toy robot. Each operation returns a bool and leaves state unchanged when
/// it cannot be applied (off-table move/placement, or acting before placement)
/// rather than throwing — so the caller can ignore invalid commands and keep
/// processing the rest.
/// </summary>
public class Robot(ITable table)
{
    public Position? Position { get; private set; }
    public Direction? Direction { get; private set; }
    
    public bool HasPosition => Position != null;

    public bool TryPlace(Position position, Direction direction)
    {
        if (!table.CanOccupy(position))
        {
            return false;
        }
        
        Position = position;
        Direction = direction;
        return true;
    }

    public bool TryMove()
    {
        if (!HasPosition)
        {
            return false;
        }

        var nextPosition = Direction!.Value.NextPosition(Position!);

        if (!table.CanOccupy(nextPosition))
        {
            return false;
        }

        Position = nextPosition;
        return true;
    }

    public bool TryTurnLeft() => TryTurn(DirectionExtensions.TurnLeft);

    public bool TryTurnRight() => TryTurn(DirectionExtensions.TurnRight);

    private bool TryTurn(Func<Direction, Direction> turn)
    {
        if (!HasPosition)
        {
            return false;
        }

        Direction = turn(Direction!.Value);
        return true;
    }
}

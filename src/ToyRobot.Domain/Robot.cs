namespace ToyRobot.Domain;

public class Robot(Table table)
{
    public Position? Position { get; private set; }
    public Direction? Direction { get; private set; }
    
    public bool HasPosition => Position != null;

    public bool TryPlace(Position position, Direction direction)
    {
        if (!table.IsWithinBounds(position))
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

        if (!table.IsWithinBounds(nextPosition))
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

namespace ToyRobot.Domain;

public class Robot(Table table)
{
    public Position? Position { get; private set; }
    public Direction? Direction { get; private set; }
    
    public bool HasPosition => Position != null;

    public bool TrySetPosition(Position position, Direction direction)
    {
        if (!table.IsWithinBounds(position))
        {
            return false;
        }
        
        Position = position;
        Direction = direction;
        return true;
    }
}

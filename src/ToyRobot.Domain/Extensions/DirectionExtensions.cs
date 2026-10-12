namespace ToyRobot.Domain.Extensions;

public static class DirectionExtensions
{
    extension(Direction direction)
    {
        public Direction TurnLeft() => direction switch
        {
            Direction.North => Direction.West,
            Direction.West => Direction.South,
            Direction.South => Direction.East,
            Direction.East => Direction.North,
            _ => direction
        };

        public Direction TurnRight() => direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => direction
        };

        // Origin (0,0) is the south-west corner, so North increases Y and East
        // increases X (South/West decrease them).
        public Position NextPosition(Position position) => direction switch
        {
            Direction.North => position with { Y = position.Y + 1 },
            Direction.East => position with { X = position.X + 1 },
            Direction.South => position with { Y = position.Y - 1 },
            Direction.West => position with { X = position.X - 1 },
            _ => position
        };
    }
}

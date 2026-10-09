namespace ToyRobot.Domain;

public class Table(int width, int height)
{
    public int Width { get; } =  width;
    public int Height { get; } = height;

    public bool IsWithinBounds(Position position) => 
        position.X >= 0 && 
        position.X < Width && 
        position.Y >= 0 && 
        position.Y < Height;
}

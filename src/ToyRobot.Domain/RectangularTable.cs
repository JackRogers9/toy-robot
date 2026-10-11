using ToyRobot.Domain.Interfaces;

namespace ToyRobot.Domain;

/// <summary>
/// A rectangular table spanning (0,0) to (Width-1, Height-1).
/// </summary>
public sealed class RectangularTable(int width, int height) : ITable
{
    private int Width { get; } = width;
    private int Height { get; } = height;

    public bool CanOccupy(Position position) =>
        position.X >= 0 &&
        position.X < Width &&
        position.Y >= 0 &&
        position.Y < Height;
}

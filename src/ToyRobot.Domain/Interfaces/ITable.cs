namespace ToyRobot.Domain.Interfaces;

/// <summary>
/// A table the robot moves on, of any shape. The only question the robot asks
/// is whether a position may be occupied, which also covers blocked cells
/// (obstacles) without changing this contract.
/// </summary>
public interface ITable
{
    bool CanOccupy(Position position);
}

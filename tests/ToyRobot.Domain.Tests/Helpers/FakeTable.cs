using ToyRobot.Domain.Interfaces;

namespace ToyRobot.Domain.Tests.Helpers;

/// <summary>
/// A stub <see cref="ITable"/> that lets a test control exactly which positions
/// may be occupied — so robot behaviour is verified through the table seam
/// without depending on any real table shape.
/// </summary>
public sealed class FakeTable : ITable
{
    private readonly Func<Position, bool> _canOccupy;

    private FakeTable(Func<Position, bool> canOccupy) => _canOccupy = canOccupy;

    /// <summary>Every position is occupiable.</summary>
    public static FakeTable AllowingEverything() => new(_ => true);

    /// <summary>No position is occupiable.</summary>
    public static FakeTable AllowingNothing() => new(_ => false);

    /// <summary>Only the given positions are occupiable.</summary>
    public static FakeTable Allowing(params Position[] allowed) =>
        new(allowed.ToHashSet().Contains);

    public bool CanOccupy(Position position) => _canOccupy(position);
}

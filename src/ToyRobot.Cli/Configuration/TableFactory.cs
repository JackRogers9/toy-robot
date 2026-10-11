using ToyRobot.Application.Interfaces;
using ToyRobot.Domain;

namespace ToyRobot.Cli.Configuration;

/// <summary>
/// Builds the robot's <see cref="ITable"/> from configuration — the single
/// place that grows as new table shapes are added. An unrecognized shape falls
/// back to a rectangular table rather than failing.
/// </summary>
public static class TableFactory
{
    public static ITable Create(TableOptions options, IRobotOutput output)
    {
        var shape = ParseShape(options.Shape, output);

        // keep so shape list can be extended easily
        return shape switch
        {
            _ => new RectangularTable(options.Width, options.Height),
        };
    }

    private static TableShape ParseShape(string? configuredShape, IRobotOutput output)
    {
        // Absent shape is a valid default; only a present-but-unrecognized value warns.
        if (string.IsNullOrWhiteSpace(configuredShape))
        {
            return TableShape.Rectangular;
        }

        if (Enum.TryParse<TableShape>(configuredShape, ignoreCase: true, out var shape))
        {
            return shape;
        }

        output.Write(
            $"Warning: unknown table shape '{configuredShape}'. Falling back to {TableShape.Rectangular}."
        );

        return TableShape.Rectangular;
    }
}

using ToyRobot.Application.Interfaces;
using ToyRobot.Domain;
using ToyRobot.Domain.Interfaces;

namespace ToyRobot.Cli.Configuration;

/// <summary>
/// Builds the robot's <see cref="ITable"/> from configuration. The single place
/// that validates table options and selects the shape; invalid size or an
/// unrecognised shape warns and falls back rather than failing.
/// </summary>
public static class TableFactory
{
    private const int DefaultSize = 5;

    public static ITable Create(TableOptions options, IRobotOutput output)
    {
        var (width, height) = ValidateSize(options.Width, options.Height, output);
        var shape = ParseShape(options.Shape, output);

        // Switch over the shape so new shapes become additional arms here.
        return shape switch
        {
            _ => new RectangularTable(width, height),
        };
    }

    private static (int Width, int Height) ValidateSize(int width, int height, IRobotOutput output)
    {
        if (width > 0 && height > 0)
        {
            return (width, height);
        }

        output.Write(
            $"Warning: invalid table size (got {width}x{height}). " +
            $"Falling back to {DefaultSize}x{DefaultSize}."
        );

        return (DefaultSize, DefaultSize);
    }

    private static TableShape ParseShape(string? configuredShape, IRobotOutput output)
    {
        // Absent shape is a valid default; only a present-but-unrecognised value warns.
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

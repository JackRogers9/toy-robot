using ToyRobot.Application.Commands;
using ToyRobot.Application.Interfaces;
using ToyRobot.Domain;

namespace ToyRobot.Application.Handlers;

public class CommandParser : ICommandParser
{
    public IRobotCommand? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var trimmed = input.Trim();
        var separatorIndex = trimmed.IndexOf(' ');
        var keyword = separatorIndex < 0 ? trimmed : trimmed[..separatorIndex];
        var arguments = separatorIndex < 0 ? string.Empty : trimmed[(separatorIndex + 1)..].Trim();

        if (keyword.Equals("PLACE", StringComparison.OrdinalIgnoreCase))
        {
            return ParsePlace(arguments);
        }

        if (arguments.Length != 0)
        {
            return null;
        }

        return keyword switch
        {
            _ when keyword.Equals("MOVE", StringComparison.OrdinalIgnoreCase) => new MoveCommand(),
            _ when keyword.Equals("LEFT", StringComparison.OrdinalIgnoreCase) => new LeftCommand(),
            _ when keyword.Equals("RIGHT", StringComparison.OrdinalIgnoreCase) => new RightCommand(),
            _ when keyword.Equals("REPORT", StringComparison.OrdinalIgnoreCase) => new ReportCommand(),
            _ => null
        };
    }

    private static PlaceCommand? ParsePlace(string arguments)
    {
        var parts = arguments.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length != 3)
        {
            return null;
        }

        if (!int.TryParse(parts[0], out var x) || !int.TryParse(parts[1], out var y))
        {
            return null;
        }

        if (!Enum.TryParse<Direction>(parts[2], ignoreCase: true, out var direction))
        {
            return null;
        }

        return new PlaceCommand(new Position(x, y), direction);
    }
}

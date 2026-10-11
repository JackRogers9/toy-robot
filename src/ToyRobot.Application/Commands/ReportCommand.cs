using ToyRobot.Application.Interfaces;

namespace ToyRobot.Application.Commands;

public class ReportCommand : IRobotCommand
{
    public void Execute(RobotContext context)
    {
        if (
            context.Robot.Position is not { } position ||
            context.Robot.Direction is not { } direction
        )
        {
            return;
        }

        var facing = direction.ToString().ToUpperInvariant();
        context.Output.Write($"{position.X},{position.Y},{facing}");
    }
}

using ToyRobot.Application.Interfaces;

namespace ToyRobot.Application.Tests.Helpers;

public class ScriptedRobotInput(params string[] lines) : IRobotInput
{
    private readonly Queue<string> _lines = new(lines);

    public string? ReadLine() => _lines.Count > 0 ? _lines.Dequeue() : null;
}

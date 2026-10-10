using ToyRobot.Application.Interfaces;

namespace ToyRobot.Application.Tests.Helpers;

public class FakeRobotOutput : IRobotOutput
{
    public List<string> Messages { get; } = [];

    public void Write(string message) => Messages.Add(message);
}

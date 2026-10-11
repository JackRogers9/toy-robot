using ToyRobot.Domain;

namespace ToyRobot.Application.Commands;

/// <summary>
/// The receiver and collaborators a command acts on when executed: the robot
/// to drive and the output to report through.
/// </summary>
public sealed record RobotContext(Robot Robot, Interfaces.IRobotOutput Output);

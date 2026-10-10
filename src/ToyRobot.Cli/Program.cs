using Microsoft.Extensions.Configuration;
using ToyRobot.Application.Handlers;
using ToyRobot.Application.Interfaces;
using ToyRobot.Cli;
using ToyRobot.Cli.Configuration;
using ToyRobot.Domain;

var output = new ConsoleRobotOutput();

var appConfig = new ConfigurationBuilder().LoadAppConfig(output);

var table = new Table(appConfig.Table.Width, appConfig.Table.Height);
var robot = new Robot(table);
ICommandParser parser = new CommandParser();
var executor = new CommandExecutor(robot, output);

output.Write("Toy Robot! Enter commands: PLACE X,Y,DIRECTION | MOVE | LEFT | RIGHT | REPORT");

while (Console.ReadLine() is { } input)
{
    var command = parser.Parse(input);
    if (command is not null)
    {
        executor.Execute(command);
    }
}

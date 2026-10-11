using Microsoft.Extensions.Configuration;
using ToyRobot.Application.Handlers;
using ToyRobot.Application.Interfaces;
using ToyRobot.Cli;
using ToyRobot.Cli.Configuration;
using ToyRobot.Domain;

using var cancellationHandler = new ConsoleCancellationHandler();

var output = new ConsoleRobotOutput();
var input = new ConsoleRobotInput(cancellationHandler.Token);

var appConfig = new ConfigurationBuilder().LoadAppConfig(output);

var robot = new Robot(TableFactory.Create(appConfig.Table, output));
var executor = new CommandExecutor(robot, output);

ICommandParser parser = new CommandParser();

new RobotConsole(input, output, parser, executor).Run();

return 0;

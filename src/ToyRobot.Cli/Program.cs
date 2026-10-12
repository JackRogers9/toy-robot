using Microsoft.Extensions.Configuration;
using ToyRobot.Application.Handlers;
using ToyRobot.Cli;
using ToyRobot.Cli.Configuration;
using ToyRobot.Domain;

using var cancellationHandler = new ConsoleCancellationHandler();

var output = new ConsoleRobotOutput();
var input = new ConsoleRobotInput(cancellationHandler.Token);

var appConfig = new ConfigurationBuilder().LoadAppConfig(output);

var table = TableFactory.Create(appConfig.Table, output);
var robot = new Robot(table);

var executor = new CommandExecutor(robot, output);

var parser = new CommandParser();

new RobotConsole(input, output, parser, executor).Run();

return 0;

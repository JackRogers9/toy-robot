using Microsoft.Extensions.Configuration;
using ToyRobot.Application.Interfaces;

namespace ToyRobot.Cli.Configuration;

public static class ConfigurationExtensions
{
    private const int DefaultTableSize = 5;

    public static AppConfig LoadAppConfig(this IConfigurationBuilder builder, IRobotOutput output)
    {
        var configuration = builder
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        var appConfig = configuration.Get<AppConfig>() ?? new AppConfig();

        if (appConfig.Table is { Width: > 0, Height: > 0 })
        {
            return appConfig;
        }
        
        output.Write(
            $"Warning: invalid or missing table configuration (got {appConfig.Table.Width}x{appConfig.Table.Height}). " +
            $"Falling back to {DefaultTableSize}x{DefaultTableSize}."
        );

        return new AppConfig
        {
            Table = new TableOptions
            {
                Shape = appConfig.Table.Shape,
                Width = DefaultTableSize, 
                Height = DefaultTableSize
            }
        };
    }
}

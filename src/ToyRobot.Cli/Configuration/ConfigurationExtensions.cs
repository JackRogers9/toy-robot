using Microsoft.Extensions.Configuration;

namespace ToyRobot.Cli.Configuration;

public static class ConfigurationExtensions
{
    public static AppConfig LoadAppConfig(this IConfigurationBuilder builder)
    {
        var configuration = builder
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();

        return configuration.Get<AppConfig>() ?? new AppConfig();
    }
}

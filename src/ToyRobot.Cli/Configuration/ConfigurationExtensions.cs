using Microsoft.Extensions.Configuration;

namespace ToyRobot.Cli.Configuration;

public static class ConfigurationExtensions
{
    public static AppConfig LoadAppConfig(this IConfigurationBuilder builder)
    {
        var configuration = builder
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        return configuration.Get<AppConfig>() ?? new AppConfig();
    }
}

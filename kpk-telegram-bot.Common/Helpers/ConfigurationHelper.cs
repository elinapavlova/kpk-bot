using Microsoft.Extensions.Configuration;

namespace kpk_telegram_bot.Common.Helpers;

public static class ConfigurationHelper
{
    public static T GetOptions<T>(this IConfiguration configuration, string key)
        where T : new()
    {
        var value = new T();
        configuration.Bind(key, value);
        return value;
    }

    public static IConfigurationRoot Build()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Dev";
        return new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{environment}.json", true)
            .Build();
    }
}
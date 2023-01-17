using kpk_telegram_bot.Common.Contracts.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Common.Extensions;

public static class DependencyProviderExtensions
{
    public static void Register<T>(this IServiceCollection services)
        where T : IDependencyProvider, new()
    {
        new T().Register(services);
    }

    public static void Register<T>(this IServiceCollection services, IConfiguration configuration)
        where T : IDependencyProviderWithConfig, new()
    {
        new T().Register(services, configuration);
    }
}
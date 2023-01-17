using kpk_telegram_bot.Common.Contracts.Providers;
using kpk_telegram_bot.Common.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Core.Providers;

public class LoggerProvider : IDependencyProvider
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<ILogger, Logger>();
    }
}
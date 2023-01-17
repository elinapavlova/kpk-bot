using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Common.Contracts.Providers;

public interface IDependencyProviderWithConfig
{
    Task Register(IServiceCollection services, IConfiguration configuration);
}
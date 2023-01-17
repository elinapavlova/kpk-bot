using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Common.Contracts.Providers;

public interface IDependencyProvider
{
    void Register(IServiceCollection services);
}
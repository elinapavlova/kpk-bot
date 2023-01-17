using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Providers;
using kpk_telegram_bot.Core.HttpClients;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Core.Providers;

public class HttpClientsProvider : IDependencyProvider
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<ITelegramHttpClient, TelegramHttpClient>();
        services.AddHttpClient<IGoogleHttpClient, GoogleHttpClient>();
    }
}
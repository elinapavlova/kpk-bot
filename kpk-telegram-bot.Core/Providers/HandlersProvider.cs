using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.Providers;
using kpk_telegram_bot.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Core.Providers;

public class HandlersProvider : IDependencyProvider
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<ITelegramBotHandler, TelegramBotHandler>();
    }
}
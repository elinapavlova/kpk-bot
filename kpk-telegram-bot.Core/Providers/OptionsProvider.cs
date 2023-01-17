using kpk_telegram_bot.Common.Contracts.Providers;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Core.Providers;

public class OptionsProvider : IDependencyProviderWithConfig
{
    public async Task Register(IServiceCollection services, IConfiguration configuration)
    {
        var telegramBotOptions = configuration.GetOptions<TelegramBotOptions>("TelegramBot");
        services.AddSingleton(telegramBotOptions);

        var appOptions = configuration.GetOptions<ApplicationOptions>("Application");
        services.AddSingleton(appOptions);

        var googleDriveApiOptions = configuration.GetOptions<GoogleDriveApiOptions>("GoogleDriveApi");
        services.AddSingleton(googleDriveApiOptions);

        var scheduleInfoOptions = configuration.GetOptions<ScheduleInfoOptions>("ScheduleInfo");
        services.AddSingleton(scheduleInfoOptions);
    }
}
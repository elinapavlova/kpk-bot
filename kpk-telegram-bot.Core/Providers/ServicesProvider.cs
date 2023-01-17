using kpk_telegram_bot.Common.Contracts.Providers;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Core.Providers;

public class ServicesProvider : IDependencyProvider
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<IBotService, BotService>();
        services.AddScoped<ICommandService, CommandService>();
        services.AddScoped<ITelegramApiService, TelegramApiService>();
        services.AddScoped<IGoogleDriveService, GoogleDriveService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
    }
}
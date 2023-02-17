using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.Providers;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Core.Import;
using kpk_telegram_bot.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Core.Providers;

public class ServicesProvider : IDependencyProvider
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<IBaseService, BaseService>();
        services.AddScoped<IBotService, BotService>();
        services.AddScoped<ICommandService, CommandService>();
        services.AddScoped<IGoogleDriveService, GoogleDriveService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IReader, PlaneReader>();
        services.AddScoped<IItemPropertyService, ItemPropertyService>();
        services.AddScoped<IPlainService, PlainService>();
        services.AddScoped<IRequestService, RequestService>();
    }
}
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.Providers;
using kpk_telegram_bot.Core.Commands;
using kpk_telegram_bot.Core.Commands.Schedule;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Core.Providers;

public class CommandsProvider : IDependencyProvider
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<ICommand, ScheduleCommand>();
        services.AddScoped<ICommand, StartCommand>();
        services.AddScoped<ICommand, MyMarksCommand>();
        
        services.AddScoped<CommandContainer>();
    }
}
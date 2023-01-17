
using kpk_telegram_bot.Common.Enums;
using Telegram.Bot.Types.InputFiles;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IScheduleService
{
    Task<List<InputOnlineFile>?> GetSchedule(ScheduleType type);
}
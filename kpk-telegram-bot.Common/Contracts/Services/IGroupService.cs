using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IGroupService
{
    Task<GroupResponse?> GetByName(string name);
}
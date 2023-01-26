using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IGroupService
{
    Task<GroupResponse?> GetByName(string name);
    Task<List<GroupResponse>?> GetAll();
    Task<GroupResponse?> Delete(string name);
}
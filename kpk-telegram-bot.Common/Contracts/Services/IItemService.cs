using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IItemService
{
    Task<ItemResponse?> GetByName(string typeName, string name);
    Task<List<ItemResponse>?> GetAll(string name, bool onlyActual = true);
    Task<ItemResponse?> Delete(string typeName, string name);
}
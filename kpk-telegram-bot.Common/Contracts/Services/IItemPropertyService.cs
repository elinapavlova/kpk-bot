using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IItemPropertyService
{
    Task AddList(Guid itemId, List<ItemPropertyCreateModel> items);
    Task<PropertyResponse?> Update(ItemPropertyEntity item);
}
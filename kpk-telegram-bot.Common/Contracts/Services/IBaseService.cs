using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Params;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IBaseService
{
    Task<IQueryable<ItemEntity>> Filter(FilterParam param);
    Task<Guid?> GetTypeIdByName(string name);
}
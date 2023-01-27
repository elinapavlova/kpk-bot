using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;

namespace kpk_telegram_bot.Common.Contracts.Repositories;

public interface IItemPropertyRepository : IBaseRepository<ItemPropertyEntity, Guid>
{
    Task<ItemPropertyEntity?> GetById(Guid itemId);
    Task<ItemPropertyEntity?> Create(ItemPropertyEntity newItem);
    Task<ItemPropertyEntity?> Update(ItemPropertyEntity itemForUpdate);
}
using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;

namespace kpk_telegram_bot.Common.Contracts.Repositories;

public interface IItemTypeRepository : IBaseRepository<ItemTypeEntity, Guid>
{
    Task<ItemTypeEntity?> GetById(Guid itemId);
    Task<ItemTypeEntity?> Create(ItemTypeEntity newItem);
    Task<ItemTypeEntity?> Update(ItemTypeEntity itemForUpdate);
    Task<ItemTypeEntity?> GetByName(string name);
}
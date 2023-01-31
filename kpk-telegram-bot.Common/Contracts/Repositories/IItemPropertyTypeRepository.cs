using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;

namespace kpk_telegram_bot.Common.Contracts.Repositories;

public interface IItemPropertyTypeRepository : IBaseRepository<ItemPropertyTypeEntity, Guid>
{
    Task<ItemPropertyTypeEntity?> GetById(Guid itemId);
    Task<ItemPropertyTypeEntity?> Create(ItemPropertyTypeEntity newItem);
    Task<ItemPropertyTypeEntity?> Update(ItemPropertyTypeEntity itemForUpdate);
    Task<ItemPropertyTypeEntity?> GetByName(string name);
}
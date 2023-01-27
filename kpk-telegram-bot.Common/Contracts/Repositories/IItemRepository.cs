using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;

namespace kpk_telegram_bot.Common.Contracts.Repositories;

public interface IItemRepository : IBaseRepository<ItemEntity, Guid>
{
    Task<ItemEntity?> GetById(Guid itemId);
    Task<ItemEntity?> Create(ItemEntity newItem);
    Task<ItemEntity?> Update(ItemEntity itemForUpdate);
    Task<IQueryable<ItemEntity>> GetByTypeId(Guid typeId);
    Task<IQueryable<ItemEntity>> GetAll();
}
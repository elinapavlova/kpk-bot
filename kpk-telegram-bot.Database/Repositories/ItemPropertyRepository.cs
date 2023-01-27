using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Database;
using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Repositories.Repositories;

public class ItemPropertyRepository : BaseRepository<ItemPropertyEntity, Guid>, IItemPropertyRepository
{
    public ItemPropertyRepository(KpkTelegramBotContext context) : base(context)
    {
    }

    public async Task<ItemPropertyEntity?> GetById(Guid itemId)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var item = await dbSet
                .Where(x => x.Id == itemId)
                .Include(x => x.Item)
                .Include(x => x.Type)
                .FirstOrDefaultAsync();
            
            return item;
        });
    }

    public async Task<ItemPropertyEntity?> Create(ItemPropertyEntity newItem)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var isItemExist = await dbSet.AnyAsync(x => x.Id == newItem.Id);
            if (isItemExist)
            {
                return null;
            }
            
            var entry = await dbSet.AddAsync(newItem);
            return entry.Entity;
        });
    }

    public async Task<ItemPropertyEntity?> Update(ItemPropertyEntity itemForUpdate)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var item = await dbSet.FirstOrDefaultAsync(x => x.Id == itemForUpdate.Id);
            if (item is null)
            {
                return null;
            }

            item.Value = itemForUpdate.Value;
            item.DateUpdated = DateTime.Now;
            item.DateDeleted = itemForUpdate.DateDeleted;
            item.IsDeleted = itemForUpdate.IsDeleted;

            var result = dbSet.Update(item);
            return result.Entity;
        });
    }
}
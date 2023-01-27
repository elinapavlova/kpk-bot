using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Database;
using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Repositories.Repositories;

public class ItemTypeRepository : BaseRepository<ItemTypeEntity, Guid>, IItemTypeRepository
{
    public ItemTypeRepository(KpkTelegramBotContext context) : base(context)
    {
    }

    public async Task<ItemTypeEntity?> GetById(Guid itemId)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var item = await dbSet
                .Where(x => x.Id == itemId)
                .Include(x => x.Items)
                .FirstOrDefaultAsync();
            
            return item;
        });
    }

    public async Task<ItemTypeEntity?> Create(ItemTypeEntity newItem)
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

    public async Task<ItemTypeEntity?> Update(ItemTypeEntity itemForUpdate)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var item = await dbSet.FirstOrDefaultAsync(x => x.Id == itemForUpdate.Id);
            if (item is null)
            {
                return null;
            }

            item.Name = itemForUpdate.Name;
            item.Value = itemForUpdate.Value;
            item.DateUpdated = DateTime.Now;
            item.DateDeleted = itemForUpdate.DateDeleted;
            item.IsDeleted = itemForUpdate.IsDeleted;

            var result = dbSet.Update(item);
            return result.Entity;
        });
    }
    
    public async Task<ItemTypeEntity?> GetByName(string name)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var item = await dbSet
                .Where(x => x.Name.Equals(name))
                .Include(x => x.Items)
                .FirstOrDefaultAsync();

            return item;
        });
    }
}
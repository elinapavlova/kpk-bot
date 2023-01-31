using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Database;
using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Repositories.Repositories;

public class ItemRepository : BaseRepository<ItemEntity, Guid>, IItemRepository
{
    public ItemRepository(KpkTelegramBotContext context) : base(context)
    {
    }
    
    public async Task<ItemEntity?> GetById(Guid itemId)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var item = await dbSet
                .Where(x => x.Id == itemId)
                .Include(x => x.Properties)
                .Include(x => x.Type)
                .FirstOrDefaultAsync();
            
            return item;
        });
    }

    public async Task<ItemEntity?> Create(ItemEntity newItem)
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

    public async Task<ItemEntity?> Update(ItemEntity itemForUpdate)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var item = await dbSet.FirstOrDefaultAsync(x => x.Id == itemForUpdate.Id);
            if (item is null)
            {
                return null;
            }

            item.DateUpdated = DateTime.Now;
            item.DateDeleted = itemForUpdate.DateDeleted;
            item.IsDeleted = itemForUpdate.IsDeleted;

            var result = dbSet.Update(item);
            return result.Entity;
        });
    }
    
    public async Task<IQueryable<ItemEntity>> GetByTypeId(Guid typeId)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var items = dbSet
                .Where(x => x.TypeId == typeId)
                .Include(x => x.Properties)
                .ThenInclude(x => x.Type)
                .Include(x => x.Type)
                .AsQueryable();
            
            return items;
        });
    }

    public async Task<IQueryable<ItemEntity>> GetAll()
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var items = dbSet
                .Include(x => x.Properties)
                .ThenInclude(x => x.Type)
                .Include(x => x.Type)
                .Include(x => x.Childs)
                .ThenInclude(x => x.Properties)
                .ThenInclude(x => x.Type)
                .AsQueryable();
            
            return items;
        });
    }
}
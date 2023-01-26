using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Database;
using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Repositories.Repositories;

public class GroupRepository : BaseRepository<GroupEntity, Guid>, IGroupRepository
{
    public GroupRepository(KpkTelegramBotContext context) : base(context)
    {
    }

    public async Task<GroupEntity?> GetByName(string name)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var group = await dbSet.FirstOrDefaultAsync(x => x.Name == name && x.DateDeleted == null);
            return group;
        });
    }
    
    public async Task<IQueryable<GroupEntity>?> GetAll()
    {
        return await ExecuteWithResult(async dbSet => dbSet.AsQueryable());
    }

    public async Task<GroupEntity?> Delete(string name)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var group = await dbSet.FirstOrDefaultAsync(x => x.Name == name);
            if (group is null)
            {
                return null;
            }
            
            group.DateDeleted ??= DateTime.Now;
            group = dbSet.Update(group).Entity;
            
            return group;
        });
    }
}
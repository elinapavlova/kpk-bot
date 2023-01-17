using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Database;
using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Repositories.Repositories;

public class UserRepository : BaseRepository<UserEntity, long>, IUserRepository
{
    public UserRepository(KpkTelegramBotContext context) : base(context)
    {
    }

    public async Task<UserEntity?> GetById(long userId)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var user = await dbSet.FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        });
    }    
    
    public async Task<bool> IsExist(long userId)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var user = await dbSet.AnyAsync(x => x.Id == userId);
            return user;
        });
    }
    
    public async Task<UserEntity?> Create(UserEntity user)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var isUserExist = await dbSet.AnyAsync(x => x.Id == user.Id);
            if (isUserExist)
            {
                return null;
            }
            
            var entry = await dbSet.AddAsync(user);
            return entry.Entity;
        });
    }  
}
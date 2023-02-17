using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Database;
using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Repositories.Repositories;

public class RequestRepository : BaseRepository<RequestEntity, Guid>, IRequestRepository
{
    public RequestRepository(KpkTelegramBotContext context) : base(context)
    {
    }

    public async Task<RequestEntity?> GetById(Guid requestId)
    {
        return await ExecuteWithoutSavingResult(async dbSet =>
        {
            var result = await dbSet
                .Where(x => x.Id == requestId)
                .Include(x => x.Group)
                .ThenInclude(x => x.Properties)
                .ThenInclude(x => x.Type)
                .FirstOrDefaultAsync();
            
            return result;
        });
    }

    public async Task<RequestEntity> Create(RequestEntity request)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var requestExisting = await dbSet.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            if (requestExisting is not null)
            {
                if (requestExisting.IsDeleted)
                {
                    requestExisting = await Update(requestExisting);
                }
                return requestExisting;
            }
            
            var entry = await dbSet.AddAsync(request);
            return entry.Entity;
        });
    }

    public async Task<RequestEntity> Update(RequestEntity request)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var requestExisting = await dbSet.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            
            requestExisting.IsDeleted = false;
            requestExisting.DateDeleted = null;
            requestExisting.DateUpdated = DateTime.Now;

            var entry = dbSet.Update(requestExisting);
            return entry.Entity;
        });

    }

    public async Task<RequestEntity?> Delete(Guid requestId)
    {
        return await ExecuteWithResult(async dbSet =>
        {
            var request = await dbSet.FirstOrDefaultAsync(x => x.Id == requestId);
            if (request is null)
            {
                return null;
            }
            
            request.DateDeleted = DateTime.Now;
            request.IsDeleted = true;
            var entry = dbSet.Update(request);
            return entry.Entity;
        });
    }
}
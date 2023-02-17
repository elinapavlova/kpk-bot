using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;

namespace kpk_telegram_bot.Common.Contracts.Repositories;

public interface IRequestRepository : IBaseRepository<RequestEntity, Guid>
{
    Task<RequestEntity?> GetById(Guid requestId);
    Task<RequestEntity> Create(RequestEntity request);
    Task<RequestEntity?> Delete(Guid requestId);
}
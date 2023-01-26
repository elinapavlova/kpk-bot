using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;

namespace kpk_telegram_bot.Common.Contracts.Repositories;

public interface IGroupRepository : IBaseRepository<GroupEntity, Guid>
{
    Task<GroupEntity?> GetByName(string name);
    Task<IQueryable<GroupEntity>?> GetAll();
    Task<GroupEntity?> Delete(string name);
}
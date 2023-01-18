using kpk_telegram_bot.Common.Database.Base;
using kpk_telegram_bot.Common.Database.Entities;

namespace kpk_telegram_bot.Common.Contracts.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity, long>
{
    Task<UserEntity?> GetById(long userId);
    Task<bool> IsExist(long userId);
    Task<UserEntity?> Create(UserEntity user);
    Task<UserEntity?> Update(UserEntity userForUpdate);
}
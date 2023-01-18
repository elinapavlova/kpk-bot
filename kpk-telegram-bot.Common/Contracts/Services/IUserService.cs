using kpk_telegram_bot.Common.Database.Entities;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IUserService
{
    Task<UserEntity?> CreateOrUpdate(UserEntity user);
    Task<bool> CheckExist(long userId);
    Task<UserEntity?> GetById(long userId);
    Task StopBot(long userId);
    Task<bool?> IsActual(long userId);
}
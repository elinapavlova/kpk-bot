using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IUserService
{
    Task<UserResponse?> Create(UserEntity user);
    Task<bool> CheckExist(long userId);
    Task<UserResponse?> GetById(long userId);
}
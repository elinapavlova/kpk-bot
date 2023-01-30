using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Logger;

namespace kpk_telegram_bot.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;

    public UserService(IUserRepository userRepository, ILogger logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserEntity?> CreateOrUpdate(UserEntity user)
    {
        var existingUser = await _userRepository.GetById(user.Id);
        if (existingUser is null)
        {
            existingUser = await _userRepository.Create(user);
            return existingUser;
        }

        existingUser.UserName = user.UserName;
        existingUser.GroupId = user.GroupId;
        existingUser.RoleId = user.RoleId;
        existingUser.DateUpdated = DateTime.Now;
        existingUser.DateDeleted = user.DateDeleted;

        existingUser = await _userRepository.Update(existingUser);
        
        return existingUser;
    }

    public async Task<bool> CheckExist(long userId)
    {
        return await _userRepository.IsExist(userId);
    }

    public async Task<UserEntity?> GetById(long userId)
    {
        var user = await _userRepository.GetById(userId);
        return user;
    }

    public async Task StopBot(long userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user is null)
        {
            _logger.Error($"Попытка выхода пользователя, которого нет в БД {userId}");
            return;
        }

        user.IsDeleted = true;
        user.DateDeleted = DateTime.Now;
        
        var result = await _userRepository.Update(user);
        if (result is null)
        {
            _logger.Error("Не удалось обновить пользователя с Id {userId}", user.Id);
            return;
        }
        
        _logger.Information("Пользователь с Id {userId} остановил бот", user.Id);
    }

    public async Task<bool?> IsActual(long userId)
    {
        var user = await _userRepository.GetById(userId);
        return user is null
            ? null
            : user.DateDeleted.HasValue is false;
    }
}
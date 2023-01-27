using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Models;

namespace kpk_telegram_bot.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IItemService _groupService;
    private readonly ILogger _logger;

    public AuthService(IUserService userService, IItemService groupService, ILogger logger)
    {
        _userService = userService;
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<string> Register(RegisterModel register)
    {
        var username = register.Info[0] + " " + register.Info[1];
        var group = await _groupService.GetByName(ItemTypeNames.Group, register.Info[2]);
        if (group is null)
        {
            return "Несуществующая группа! Попробуйте еще раз";
        }
        
        var user = await _userService.CreateOrUpdate(new UserEntity
        {
            Id = register.Id, UserName = username, GroupId = group.Id, RoleId = (uint)UserRole.Student
        });

        if (user is null)
        {
            return "Что-то пошло не так! Попробуйте еще раз";
        }
        _logger.Information("Регистрация пользователя {id} [{username}] {groupName}", register.Id, username, group.Name);
        
        var result = UserMapper.Map(user);
        
        return $"Вы добавлены в базу!\r\nСтудент: {result.UserName}\r\nГруппа: {result.GroupName}" +
               "\r\nЕсли что-то пошло не так, свяжитесь с @otstante_mne_grustno";
    }

    public async Task Restart(long userId)
    {
        var user = await _userService.GetById(userId);
        if (user is null)
        {
            _logger.Error("Перезапуск бота. Пользователь по Id {userId} не найден", userId);
            return;
        }

        user.DateDeleted = null;
        user = await _userService.CreateOrUpdate(user);
        
        if (user is null)
        {
            _logger.Error("Перезапуск бота. Пользователя по Id {userId} не удалось обновить", userId);
            return;
        }
        
        _logger.Information("Пользователь с Id {userId} перезапустил бот", userId);
    }
}
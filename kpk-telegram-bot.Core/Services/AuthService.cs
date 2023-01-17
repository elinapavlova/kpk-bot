using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Models;

namespace kpk_telegram_bot.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IGroupService _groupService;
    private readonly ILogger _logger;

    public AuthService(IUserService userService, IGroupService groupService, ILogger logger)
    {
        _userService = userService;
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<string> Register(RegisterModel register)
    {
        var username = register.Info[0] + " " + register.Info[1];
        var group = await _groupService.GetByName(register.Info[2]);
        if (group is null)
        {
            return "Несуществующая группа! Попробуйте еще раз";
        }
        
        var user = await _userService.Create(new UserEntity
        {
            Id = register.Id, UserName = username, GroupId = group.Id, RoleId = (uint)UserRole.Student
        });

        if (user is null)
        {
            return "Что-то пошло не так! Попробуйте еще раз";
        }
        _logger.Information($"Register user {register.Id} [{username}] {group.Name}");
        
        return $"Вы добавлены в базу!\r\nСтудент: {user.UserName}\r\nГруппа: {user.GroupName}" +
               "\r\nЕсли что-то пошло не так, свяжитесь с @otstante_mne_grustno";
    }
}
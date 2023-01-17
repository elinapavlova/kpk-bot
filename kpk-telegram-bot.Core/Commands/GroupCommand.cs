using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Common.Logger;
using Telegram.Bot.Types;

namespace kpk_telegram_bot.Core.Commands;

public class GroupCommand : ICommand
{
    private readonly ITelegramHttpClient _telegramHttpClient;
    private readonly ILogger _logger;
    private readonly IUserService _userService;

    public GroupCommand
    (
        ITelegramHttpClient telegramHttpClient, 
        ILogger logger,
        IUserService userService
    )
    {
        _telegramHttpClient = telegramHttpClient;
        _logger = logger;
        _userService = userService;
    }
    
    public async Task Execute(Message message)
    {
        var user = await _userService.GetById(message.From.Id);
        if (RoleHelper.CheckRole(user, new List<UserRole> {UserRole.Admin}) is false)
        {
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Недостаточно прав для управления группами");
            return;
        }
        
    }
}
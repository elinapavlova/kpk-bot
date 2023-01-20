using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Core.Commands;

public class StartCommand : ICommand
{
    private readonly ITelegramHttpClient _telegramHttpClient;
    private readonly IUserService _userService;

    public StartCommand(ITelegramHttpClient telegramHttpClient, IUserService userService)
    {
        _telegramHttpClient = telegramHttpClient;
        _userService = userService;
    }
    
    public async Task Execute(Message message)
    {
        ReplyKeyboardMarkup? keyboard;
        
        var user = await _userService.GetById(message.From.Id);
        if (user is null)
        {
            keyboard = CreateCommandsKeyboardByRole(UserRole.Student);
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Меню", keyboard);
            return;
        }
        
        keyboard = CreateCommandsKeyboardByRole((UserRole)user.RoleId);
        await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Меню", keyboard);
    }
    
    private static ReplyKeyboardMarkup CreateCommandsKeyboardByRole(UserRole userRole)
    {
        var rows = new List<KeyboardButton[]>();
        var columns = new List<KeyboardButton>();
        var lastIndex = UsefulCommands.Commands(userRole).Count - 1;
        var position = 0;
        
        foreach (var name in UsefulCommands.Commands(userRole))
        {
            columns.Add(name.Key);
                
            if (position % 2 == 0 && position != lastIndex)
            {
                position++;
                continue;
            }
            
            position++;
            rows.Add(columns.ToArray());
            columns = new List<KeyboardButton>();
        }
        
        var keyboard = new ReplyKeyboardMarkup(oneTimeKeyboard: true, keyboard: rows.ToArray());
        return keyboard;
    }
}
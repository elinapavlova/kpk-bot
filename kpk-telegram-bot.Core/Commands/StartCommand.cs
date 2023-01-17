using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Core.Commands;

public class StartCommand : ICommand
{
    private readonly ITelegramHttpClient _telegramHttpClient;

    public StartCommand(ITelegramHttpClient telegramHttpClient)
    {
        _telegramHttpClient = telegramHttpClient;
    }
    
    public async Task Execute(Message message)
    {
        var keyboard = CreateCommandsKeyboard();
        await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Меню", keyboard);
    }

    private static ReplyKeyboardMarkup CreateCommandsKeyboard()
    {
        var keyboard = new ReplyKeyboardMarkup();
        var rows = new List<KeyboardButton[]>();
        var columns = new List<KeyboardButton>();
        var lastIndex = UsefulCommands.Commands.Count - 1;
        var position = 0;
        
        foreach (var name in UsefulCommands.Commands)
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

        keyboard.Keyboard = rows.ToArray();
        return keyboard;
    }
}
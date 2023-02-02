using kpk_telegram_bot.Common.Consts.Keyboards;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Helpers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Core.Commands;

public class PlainCommand : ICommand
{
    private readonly IUserService _userService;
    private readonly IPlainService _plainService;
    private readonly ITelegramHttpClient _telegramHttpClient;

    public PlainCommand(IUserService userService, ITelegramHttpClient telegramHttpClient, IPlainService plainService)
    {
        _userService = userService;
        _telegramHttpClient = telegramHttpClient;
        _plainService = plainService;
    }
    
    public async Task Execute(Message message)
    {
        var text = message.Text.Trim();
        var words = text.Split(' ');

        var user = await _userService.GetById(message.From.Id);
        if (user is null)
        {
            CommandHelper.ThrowException(string.Empty, text);
            return;
        }
        
        var subjects = await _plainService.GetSubjectNames();

        if (text == "Учебный план")
        {
            var keyboard = CreateCommandsKeyboard((UserRole)user.RoleId, subjects);
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Учебный план", keyboard);
            return;
        }
        
        words = words.First().Split('_');
        if (words.Length != 2)
        {
            CommandHelper.ThrowException(string.Empty, text);
            return;
        }

        var subject = subjects.FirstOrDefault(x => x.Key == Guid.Parse(words[1]));

        var result = await _plainService.GetPlainBySubjectId(subject.Key);
        await _telegramHttpClient.SendTextMessage(message.Chat.Id, result);
    }
    
    private static InlineKeyboardMarkup CreateCommandsKeyboard(UserRole userRole, Dictionary<Guid, string> subjects)
    {
        var keyboard = new InlineKeyboardMarkup(PlainKeyboardCommands.GetInlineButtons(userRole, subjects));
        return keyboard;
    }
}
using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Mappers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Core.Commands;

public class ScheduleCommand : ICommand
{
    private readonly IScheduleService _scheduleService;
    private readonly IUserService _userService;
    private readonly ITelegramHttpClient _telegramHttpClient;
    private readonly ILogger _logger;

    public ScheduleCommand(IScheduleService scheduleService, IUserService userService, ITelegramHttpClient telegramHttpClient, ILogger logger)
    {
        _scheduleService = scheduleService;
        _userService = userService;
        _telegramHttpClient = telegramHttpClient;
        _logger = logger;
    }
    
    public async Task Execute(Message message)
    {
        var text = message.Text.Trim();
        var words = text.Split(' ');
        if (words.Length != 1)
        {
            await HandleError(message, text);
            return;
        }
        
        var user = await _userService.GetById(message.From.Id);
        if (user is null)
        {
            await HandleError(message, text);
            return;
        }
        if (text == "Расписание")
        {
            var keyboard = CreateCommandsKeyboard((UserRole)user.RoleId);
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Расписание", keyboard);
            return;
        }

        words = words.First().Split('_');
        if (words.Length != 2)
        {
            await HandleError(message, text);
            return;
        }

        var schedule = await _scheduleService.GetSchedule(ScheduleTypeMapper.Map(words[1]));
        if (schedule is null)
        {
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Расписание отсутствует");
            return;
        }
        await _telegramHttpClient.SendPhotoMessage(message.Chat.Id, ScheduleHelper.CreateMessageText(words[1]), schedule);
    }

    private async Task HandleError(Message message, string text)
    {
        await _telegramHttpClient.SendTextMessage(message.Chat.Id,
            "Попробуйте перезапустить бот с помощью команды /start и попробовать снова");
        _logger.Warning("Невалидный запрос на получение расписания {text}. Пользователь {fromId} [{fromUsername}]",
            text, message.From.Id, message.From.Username);
    }

    private static InlineKeyboardMarkup CreateCommandsKeyboard(UserRole userRole)
    {
        var keyboard = new InlineKeyboardMarkup(ScheduleKeyboardCommands.GetInlineButtons(userRole));
        return keyboard;
    }
}
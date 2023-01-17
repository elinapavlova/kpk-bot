using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Mappers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Core.Commands.Schedule;

public class ScheduleCommand : ICommand
{
    private readonly IScheduleService _scheduleService;
    private readonly ITelegramHttpClient _telegramHttpClient;
    private readonly ILogger _logger;

    public ScheduleCommand(IScheduleService scheduleService, ITelegramHttpClient telegramHttpClient, ILogger logger)
    {
        _scheduleService = scheduleService;
        _telegramHttpClient = telegramHttpClient;
        _logger = logger;
    }
    
    public async Task Execute(Message message)
    {
        var text = message.Text.Trim();
        if (text == "Расписание")
        {
            var keyboard = CreateCommandsKeyboard();
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Расписание", keyboard);
            return;
        }
        
        var words = text.Split(' ');
        if (words.Length != 1)
        {
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Невалидный запрос на получение расписания");
            _logger.Warning("Невалидный запрос на получение расписания {text}. Пользователь {fromId} [{fromUsername}]", 
                text, message.From.Id, message.From.Username);
            return;
        }
        
        words = words.First().Split('_');
        if (words.Length != 2)
        {
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Невалидный запрос на получение расписания");
            _logger.Warning("Невалидная команда {text}. Пользователь {fromId} [{fromUsername}]", 
                text, message.From.Id, message.From.Username);
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
    
    private static InlineKeyboardMarkup CreateCommandsKeyboard()
    {
        var keyboard = new InlineKeyboardMarkup(
            new InlineKeyboardButton[] 
            {//TODO по ролям (очник/заочник/преподаватель)
                new () 
                {
                    Text = "Сегодня", CallbackData = "/schedule_today"
                },
                new ()
                {
                    Text = "Актуальное", CallbackData = "/schedule_actual"
                },
                new ()
                {
                    Text = "Завтра", CallbackData = "/schedule_tomorrow"
                },                
                new ()
                {
                    Text = "Неделя", CallbackData = "/schedule_week"
                },
                // new ()
                // {
                //     Text = "Заочники", CallbackData = "/schedule_distant"
                // }
            }
        );
        return keyboard;
    }
}
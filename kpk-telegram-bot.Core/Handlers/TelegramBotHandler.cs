using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace kpk_telegram_bot.Core.Handlers;

public class TelegramBotHandler : ITelegramBotHandler
{
    private readonly ILogger _logger;
    private readonly ITelegramHttpClient _telegramHttpClient;
    private readonly ICommandService _commandService;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    
    public TelegramBotHandler
    (
        ILogger logger, 
        ITelegramHttpClient telegramHttpClient,
        ICommandService commandService,
        IUserService userService,
        IAuthService authService
    )
    {
        _logger = logger;
        _telegramHttpClient = telegramHttpClient;
        _commandService = commandService;
        _userService = userService;
        _authService = authService;
    }
        
    public DefaultUpdateHandler CreateDefaultUpdateHandler()
        => new (HandleUpdateAsync, HandleErrorAsync);
        
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    await BotOnMessageReceived(UpdateHelper.CallbackQueryToMessage(update.CallbackQuery));
                    break;
                    
                case UpdateType.Message :
                    await BotOnMessageReceived(update.Message);
                    break;
                    
                case UpdateType.MyChatMember :
                    if (update.MyChatMember.NewChatMember.Status != ChatMemberStatus.Kicked) 
                        break;
                    await _userService.StopBot(update.MyChatMember.From.Id);
                    break;
            }
        }
        catch (Exception exception)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _telegramHttpClient.EditTextMessage(update.CallbackQuery.From.Id,
                    update.CallbackQuery.Message.MessageId, "Выбор недоступен");
                return;
            }
            await HandleErrorAsync(botClient, exception, cancellationToken);
        }
    }
        
    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => "Telegram API Error:\n" +
                                                       $"[{apiRequestException.ErrorCode}]\n" +
                                                       $"{apiRequestException.Message}",
            _                                       => exception.ToString()
        };
        _logger.Error($"Telegram API handling updates Error: {errorMessage}");
    }
        
    private async Task BotOnMessageReceived(Message message)
    {
        //TODO запрет на использование в группах
        //TODO принимать excel
        // Если сообщение не текст - игнорировать
        if (message.Type != MessageType.Text && message.Type != MessageType.Document)
        {
            return;
        }
        try
        {
            if (string.IsNullOrEmpty(message.Text) is false)
            {
                var words = message.Text?.Split(' ');
                //TODO переделать
                if (await _userService.CheckExist(message.From.Id) is false)
                {
                    if (words.Length != 3)
                    {
                        await _telegramHttpClient.SendTextMessage(message.Chat.Id,
                            "Вы новенький! Отправьте информацию о себе\r\n" +
                            "Пример: Иванов Иван 1-1П9");
                    }

                    if (words.Length != 3)
                    {
                        return;
                    }

                    var result = await _authService.Register(new RegisterModel(message.From.Id, words));
                    await _telegramHttpClient.SendTextMessage(message.Chat.Id, result);
                    return;
                }
            }
            if (await _userService.IsActual(message.From.Id) is false)
            {
                await _authService.Restart(message.From.Id);
            }
            
            await _commandService.Execute(message);
        }
        catch (Exception e)
        {
            _logger.Error("Receiving message error: {error}\r\nuser : {fromId} [ {fromUsername} ]\r\n" + 
                          "message : {text}\r\nstack trace: {stackTrace}", 
                          e.Message, message.From?.Id, message.From?.Username, message.Text, e.StackTrace ?? string.Empty);
        }
    }
}
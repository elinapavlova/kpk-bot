using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Common.Logger;
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

        if (message.Type != MessageType.Text && message.Type != MessageType.Document 
            || string.IsNullOrEmpty(message.Text) && string.IsNullOrEmpty(message.Caption))
        {
            return;
        }
        try
        {
            var isUserActual = await _userService.IsActual(message.From.Id);
            switch (isUserActual)
            {
                case false:
                    await _authService.Restart(message.From.Id);
                    break;
                case null: 
                    if (message.Text == "/start") 
                    { 
                        await _commandService.Execute(message); 
                        break; 
                    }
                    await _authService.Verify(message);
                    break;
                case true:
                    await _commandService.Execute(message);
                    break;
            }
        }
        catch (Exception e)
        {
            _logger.Error("Receiving message error: {error}\r\nuser : {fromId} [ {fromUsername} ]\r\n" + 
                          "message : {text}\r\nstack trace: {stackTrace}", 
                          e.Message, message.From?.Id, message.From?.Username, message.Text, e.StackTrace ?? string.Empty);
        }
    }
}
using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Consts.Keyboards;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IRequestService _requestService;
    private readonly ApplicationOptions _options;
    private readonly ITelegramHttpClient _telegramHttpClient; 
    private readonly ILogger _logger;

    private const string GroupNameProperty = "groupName";

    public AuthService
    (
        IUserService userService, 
        ILogger logger, 
        ApplicationOptions options, 
        ITelegramHttpClient telegramHttpClient, 
        IRequestService requestService
    )
    {
        _userService = userService;
        _logger = logger;
        _options = options;
        _telegramHttpClient = telegramHttpClient;
        _requestService = requestService;
    }

    public async Task Verify(Message message)
    {
        var words = message.Text.Split('_');
        Guid.TryParse(words[1], out var requestId);
        var existingRequest = await _requestService.GetById(requestId);
        if (existingRequest?.IsDeleted is false)
        {
            var action = message.Text.Split('_').LastOrDefault();
            await Register(existingRequest, message.From.Id, action, message.MessageId);
            return;
        }
        
        words = message.Text.Split(' ');
        if (words.Length != 3)
        {
            await SendAnswer(message.From.Id, "Что-то пошло не так. Попробуйте снова или свяжитесь с @otstante_mne_grustno");
            return;
        }
        
        var userName = words[0] + " " + words[1];
        var groupName = words[2];

        await CreateRequest(message, userName, groupName);
    }


    private async Task Register(RequestEntity request, long adminId, string action, int messageId)
    {
        var groupName = request.Group.Properties.FirstOrDefault(x => x.Type.Name == GroupNameProperty)?.Value;

        switch (action)
        {
            case RequestActions.Accept:
                await RequestAccept(request, adminId, groupName, messageId);
                return;
            case RequestActions.Cancel:
                await RequestCancel(request, adminId, groupName, messageId);
                return;
        }
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
    
        private async Task RequestAccept(RequestEntity request, long adminId, string? groupName, int messageId)
    {
        var result = await _requestService.Accept(request);
        if (result is null)
        {
            await SendAnswer(request.UserId, "Что-то пошло не так. Попробуйте снова или свяжитесь с @otstante_mne_grustno");
        }
        
        await DeactivateRequestMessage(adminId, messageId, $"Заявка {result.RequestId} подтверждена");
        await SendAnswer(request.UserId, $"Вы добавлены в базу!\r\nСтудент: {request.StudentName}\r\nГруппа: {groupName}");
        await SendAnswer(adminId, $"{request.StudentName} добавлен в группу {groupName}");

        _logger.Information("Подтверждение заявки {requestId} пользователя {id} [{username}] {groupName}",
            request.Id, request.UserId, request.StudentName, groupName);
    }
    
    private async Task RequestCancel(RequestEntity request, long adminId, string? groupName, int messageId)
    {
        var result = await _requestService.Cancel(request.Id);
        if (result is null)
        {
            await SendAnswer(request.UserId, "Что-то пошло не так. Попробуйте снова или свяжитесь с @otstante_mne_grustno");
        }
        
        await DeactivateRequestMessage(adminId, messageId, $"Заявка {result.RequestId} отклонена");
        await SendAnswer(request.UserId, "Ваша заявка отклонена\r\nЕсли что-то пошло не так, свяжитесь с @otstante_mne_grustno");

        _logger.Information("Отклонение заявки {requestId} пользователя {id} [{username}] {groupName}",
            request.Id, request.UserId, request.StudentName, groupName);
    }

    private async Task CreateRequest(Message message, string username, string groupName)
    {
        var request = await _requestService.Create(new RequestCreateModel(username, message.From.Id, groupName));

        await SendAnswer(_options.CreatorId, $"Запрос на регистрацию\r\nСтудент {username}\r\nГруппа {groupName}",
            new InlineKeyboardMarkup(VerifyStudentKeyboard.Get(request.Id)));

        await SendAnswer(message.From.Id, "Ваша заявка отправлена на рассмотрение администратору");

        _logger.Information(
            "Создана заявка {requestId} на регистрацию {username} [{userId}] ({studentName} {groupName})",
            request.Id, message.From.Username, message.From.Id, username, groupName);
    }

    private async Task SendAnswer(long userId, string message, InlineKeyboardMarkup? keyboard = null)
    {
        if (keyboard is null)
        {
            await _telegramHttpClient.SendTextMessage(userId, message);
            return;
        }
        
        await _telegramHttpClient.SendTextMessage(userId, message, keyboard);
    }

    private async Task DeactivateRequestMessage(long userId, int messageId, string text)
    {
        await _telegramHttpClient.EditTextMessage(userId, messageId, text);
    }
}
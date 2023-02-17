using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Exceptions;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Core.Commands;
using kpk_telegram_bot.Core.Helpers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace kpk_telegram_bot.Core.Services;

public class CommandService : ICommandService
{
    private readonly Dictionary<string, ICommand> _commands;
    private readonly ILogger _logger;
    private readonly ITelegramHttpClient _telegramHttpClient;
    private readonly IUserService _userService;

    public CommandService
    (
        CommandContainer container, 
        ILogger logger,
        ITelegramHttpClient telegramHttpClient,
        IUserService userService
    )
    {
        _commands = container.GetCommands();
        _logger = logger;
        _telegramHttpClient = telegramHttpClient;
        _userService = userService;
    }
    
    public async Task Execute(Message message)
    {
        message.Text ??= message.Caption;

        var commandName = message.Type == MessageType.Document 
        ? _commands.FirstOrDefault(x => x.Value.ToString().Contains(nameof(ImportCommand))).Key
        : message.Text.StartsWith('/') 
            ? message.Text.Trim().Split(' ').FirstOrDefault() 
            : message.Text.Trim();
        
        var command = GetCommandByName(commandName?.Trim());
        if (command is null)
        {
            _logger.Warning($"Команда {commandName} не обнаружена");
            return;
        }
        if (await CheckUserRoleAvailable(message.From.Id, command.GetType().Name) is not true)
        {
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, "Недостаточно прав для выполнения команды");
            return;
        }
    
        try
        {
            await command.Execute(message);
            _logger.Debug("Выполнена команда {commandName}. Пользователь {userId} [{username}]", 
                command.GetType().Name, message.From.Id, message.From.Username);
        }
        catch (CommandExecuteException e)
        {
            await _telegramHttpClient.SendTextMessage(message.Chat.Id, string.IsNullOrEmpty(e.Message) ? e.InvalidMessage : e.Message);
            _logger.Warning("Невалидный запрос {text}. Пользователь {fromId} [{fromUsername}]", 
                e.Details.GetValueOrDefault("text"), message.From.Id, message.From.Username);
        }
        catch (Exception e)
        {
            _logger.Error("Не удалось выполнить команду {commandName} пользователем {userId}\r\nСообщение: {text}\r\nException:{exception}",
                command.GetType().Name, message.From.Id, message.Text, e);
        }
    }
    
    private ICommand? GetCommandByName(string? commandName)
    {
        if (string.IsNullOrEmpty(commandName) is false && UsefulCommands.All().Any(x => x.Command.Key == commandName))
        {
            commandName = UsefulCommands.All().FirstOrDefault(x => x.Command.Key == commandName)?.Command.Value;
        }
        return string.IsNullOrEmpty(commandName) 
            ? null 
            : _commands.FirstOrDefault(x => x.Key == commandName.Split('_').First()).Value;
    }

    private async Task<bool> CheckUserRoleAvailable(long userId, string command)
    {
        var user = await _userService.GetById(userId);
        var isAvailable = RoleHelper.CheckRole(user?.RoleId ?? (int)UserRole.Guest, command);
        if (isAvailable is not true)
        {
            _logger.Warning("Недостаточно прав пользователя {userId} с ролью {role} для команды {command}",
                userId, user?.RoleId is null ? string.Empty : (UserRole)user.RoleId, command);
        }
        return isAvailable;
    }
}
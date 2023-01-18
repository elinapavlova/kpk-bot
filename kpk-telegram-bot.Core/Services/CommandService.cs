using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Logger;
using Telegram.Bot.Types;

namespace kpk_telegram_bot.Core.Services;

public class CommandService : ICommandService
{
    private readonly Dictionary<string, ICommand> _commands;
    private readonly ILogger _logger;

    public CommandService
    (
        CommandContainer container, 
        ILogger logger
    )
    {
        _commands = container.GetCommands();
        _logger = logger;
    }
    
    public async Task Execute(Message message)
    {
        var commandName = message.Text.StartsWith('/') 
            ? message.Text.Trim().Split(' ').FirstOrDefault() 
            : message.Text.Trim();
        
        var command = GetCommandByName(commandName?.Trim());
        if (command is null)
        {
            _logger.Warning($"Команда {commandName} не обнаружена");
            return;
        }

        try
        {
            await command.Execute(message);
            _logger.Debug("Successfully executed {commandName}", command.GetType().Name);
        }
        catch (Exception e)
        {
            _logger.Error("Failed execute command {commandName} by user {userId}\r\nException:{exception}",
                command.GetType().Name,message.From.Id, e);
        }
    }
    
    private ICommand? GetCommandByName(string? commandName)
    {
        if (string.IsNullOrEmpty(commandName) is false && UsefulCommands.Commands.ContainsKey(commandName))
        {
            commandName = UsefulCommands.Commands.GetValueOrDefault(commandName);
        }
        return string.IsNullOrEmpty(commandName) 
            ? null 
            : _commands.FirstOrDefault(x => x.Key == commandName.Split('_').First()).Value;
    }
}
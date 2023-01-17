using kpk_telegram_bot.Common.Options;

namespace kpk_telegram_bot.Common.Contracts.Commands;

public class CommandContainer
{
    private readonly Dictionary<string, ICommand> _commands = new ();
    private readonly Dictionary<string, string> _commandOptions;

    public CommandContainer
    (
        IEnumerable<ICommand> commands,
        TelegramBotOptions botOptions
    )
    {
        _commandOptions = botOptions.Commands;
        MapCommands(commands.ToList());
    }

    public Dictionary<string, ICommand> GetCommands()
    {
        return _commands;
    }
    
    private void MapCommands(IReadOnlyCollection<ICommand> commands)
    {
        foreach (var command in _commandOptions)
        {
            var existingCommand = commands.FirstOrDefault(x => x.GetType().Name == command.Key);
            if (existingCommand is not null && _commands.ContainsKey(command.Value) is false)
            {
                _commands.Add(command.Value, existingCommand);
            }
        }
    }
}
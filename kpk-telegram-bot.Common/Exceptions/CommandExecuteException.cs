namespace kpk_telegram_bot.Common.Exceptions;

public class CommandExecuteException : BaseException
{
    public CommandExecuteException(string message, string type) : base(message, type)
    {
    }

    public CommandExecuteException(string message, string type, Exception ex) : base(message, type, ex)
    {
    }

    public CommandExecuteException(string message, Dictionary<string, string>? details, string type = nameof(CommandExecuteException)) : base(message, type, details)
    {
    }

    public CommandExecuteException(string message) : base(message)
    {
    }

    public CommandExecuteException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
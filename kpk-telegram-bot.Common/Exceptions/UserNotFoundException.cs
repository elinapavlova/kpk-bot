namespace kpk_telegram_bot.Common.Exceptions;

public class UserNotFoundException : BaseException
{
    public UserNotFoundException(string message, string type) : base(message, type)
    {
    }

    public UserNotFoundException(string message, string type, Exception ex) : base(message, type, ex)
    {
    }

    public UserNotFoundException(string message, string type, Dictionary<string, string>? details) : base(message, type, details)
    {
    }

    public UserNotFoundException(string message) : base(message)
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
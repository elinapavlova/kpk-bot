namespace kpk_telegram_bot.Common.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message, string type) : base(message, type)
    {
    }

    public NotFoundException(string message, string type, Exception ex) : base(message, type, ex)
    {
    }

    public NotFoundException(string message, string type, Dictionary<string, string>? details) : base(message, type, details)
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
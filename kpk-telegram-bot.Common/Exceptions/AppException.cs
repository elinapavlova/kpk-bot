namespace kpk_telegram_bot.Common.Exceptions;

public class AppException : BaseException
{
    public AppException(string message, string type) : base(message, type)
    {
    }

    public AppException(string message, string type, Exception ex) : base(message, type, ex)
    {
    }

    public AppException(string message, string type, Dictionary<string, string>? details) : base(message, type, details)
    {
    }

    public AppException(string message) : base(message)
    {
    }

    public AppException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
namespace kpk_telegram_bot.Common.Exceptions;

public class BaseException : Exception
{
    public Dictionary<string, string> Details { get; set; } = new ();
    public string ExceptionType { get; set; }
    
    public BaseException(string message, string type) : this(message, type, new Dictionary<string, string>())
    {

    }

    public BaseException(string message, string type, Exception ex) : base(message,ex)
    {
        ExceptionType = type;
        Details = new Dictionary<string, string>();
    }

    public BaseException(string message, string type, Dictionary<string, string>? details) : base(message)
    {
        ExceptionType = type;
        Details = details ?? new Dictionary<string, string>();
    }

        
    public BaseException(string message) : base(message)
    {
    }

    public BaseException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
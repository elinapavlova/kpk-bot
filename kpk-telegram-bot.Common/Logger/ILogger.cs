namespace kpk_telegram_bot.Common.Logger;

public interface ILogger
{
    void UseContext(string propertyName, object value);
    void Debug(string message, params object[] propertyValues);
    void Debug(Exception ex, string message);
    void Information(string message, params object[] propertyValues);
    void Error(Exception exception, string message, params object[] propertyValues);
    void Error(string message, params object[] propertyValues);
    void Fatal(Exception exception, string message, params object[] propertyValues);
    void Warning(string message, params object[] propertyValues);
    void Warning(Exception exception, string message, params object[] propertyValues);
}
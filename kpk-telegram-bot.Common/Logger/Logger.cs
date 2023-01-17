using Serilog.Core;
using Serilog.Core.Enrichers;

namespace kpk_telegram_bot.Common.Logger;

public class Logger : ILogger
{
    private readonly Dictionary<string, object> _context;

    public Logger()
    {
        _context = new Dictionary<string, object>();
    }

    public void UseContext(string propertyName, object value)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return;
        }

        if (_context.ContainsKey(propertyName))
        {
            _context[propertyName] = value;
        }
        else
        {
            _context.Add(propertyName, value);
        }
    }

    private Serilog.ILogger Log => Serilog.Log.Logger.ForContext(GetContextualVariables());

    public void Debug(string message, params object[] propertyValues)
    {
        Log.Debug(message, propertyValues);
    }

    public void Debug(Exception ex, string message)
    {
        Log.Debug(ex, message);
    }

    public void Information(string message, params object[] propertyValues)
    {
        Log.Information(message, propertyValues);
    }

    public void Error(string message, params object[] propertyValues)
    {
        Log.Error(message, propertyValues);
    }

    public void Error(Exception exception, string message, params object[] propertyValues)
    {
        Log.Error(exception, message, propertyValues);
    }

    public void Fatal(Exception exception, string message, params object[] propertyValues)
    {
        Log.Fatal(exception, message, propertyValues);
    }

    public void Warning(string message, params object[] propertyValues)
    {
        Log.Warning(message, propertyValues);
    }

    public void Warning(Exception exception, string message, params object[] propertyValues)
    {
        Log.Warning(exception, message, propertyValues);
    }


    private IEnumerable<ILogEventEnricher> GetContextualVariables(Exception? ex = null,
        params (string, object)[] contextVariables)
    {
        var res = _context.Select(x => new PropertyEnricher(x.Key, x.Value)).ToList();
        var fullName = ex?.GetType().FullName;
        if (fullName != null)
        {
            res.Add(new PropertyEnricher("ExceptionType", fullName));
        }

        foreach (var (key, value) in contextVariables)
        {
            res.Add(new PropertyEnricher(key, value));
        }

        return res;
    }
}
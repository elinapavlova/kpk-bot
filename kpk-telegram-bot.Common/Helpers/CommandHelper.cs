using kpk_telegram_bot.Common.Exceptions;

namespace kpk_telegram_bot.Common.Helpers;

public static class CommandHelper
{
    public static void ThrowException(string message, string text)
    {
        throw new CommandExecuteException(message, details: new Dictionary<string, string>
        {
            {"text", text}
        });
    }
}
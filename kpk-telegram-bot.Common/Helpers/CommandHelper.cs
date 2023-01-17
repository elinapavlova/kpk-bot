using Telegram.Bot.Types.Enums;

namespace kpk_telegram_bot.Common.Helpers;

public static class CommandHelper
{
    public static bool IsChatGroup(ChatType chatType)
    {
        return chatType is ChatType.Group or ChatType.Supergroup;
    }
    
    public static string? Fix(string? name)
    {
        return string.IsNullOrEmpty(name)
            ? null
            : name.Replace("@kpk_telegram_bot", string.Empty);
    }
}
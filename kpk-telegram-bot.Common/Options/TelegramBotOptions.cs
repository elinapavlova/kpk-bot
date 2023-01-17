namespace kpk_telegram_bot.Common.Options;

public class TelegramBotOptions
{
    public string Token { get; set; }
    public Dictionary<string, string> Commands { get; set; }
}
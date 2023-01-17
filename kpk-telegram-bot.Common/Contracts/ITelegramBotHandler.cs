using Telegram.Bot.Extensions.Polling;

namespace kpk_telegram_bot.Common.Contracts;

public interface ITelegramBotHandler
{
    DefaultUpdateHandler CreateDefaultUpdateHandler();
}
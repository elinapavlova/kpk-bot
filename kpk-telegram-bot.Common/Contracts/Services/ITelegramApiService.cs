using Telegram.Bot.Types;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface ITelegramApiService
{
    Task StopBot(Update update);
}
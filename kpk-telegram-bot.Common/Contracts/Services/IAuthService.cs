using Telegram.Bot.Types;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IAuthService
{
    Task Restart(long userId);
    Task Verify(Message message);
}
using kpk_telegram_bot.Common.Models;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IAuthService
{
    Task<string> Register(RegisterModel register);
    Task Restart(long userId);
}
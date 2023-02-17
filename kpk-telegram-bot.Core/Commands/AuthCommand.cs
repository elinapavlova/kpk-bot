using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.Services;
using Telegram.Bot.Types;

namespace kpk_telegram_bot.Core.Commands;

public class AuthCommand : ICommand
{
    private readonly IAuthService _authService;

    public AuthCommand(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task Execute(Message message)
    {
        await _authService.Verify(message);
    }
}
using kpk_telegram_bot.Common.Contracts.Services;
using Telegram.Bot.Types;

namespace kpk_telegram_bot.Core.Services;

public class TelegramApiService : ITelegramApiService
{
    public Task HandleChosenCard(CallbackQuery callback)
    {
        throw new NotImplementedException();
    }

    public Task UploadCard(Message message)
    {
        throw new NotImplementedException();
    }

    public Task HandlePoll(Poll poll)
    {
        throw new NotImplementedException();
    }

    public Task StopBot(Update update)
    {
        throw new NotImplementedException();
    }
}
using Telegram.Bot.Types;

namespace kpk_telegram_bot.Common.Contracts.Commands;

public interface ICommand
{
    Task Execute(Message message);
}
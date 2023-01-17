using Telegram.Bot.Types;

namespace kpk_telegram_bot.Common.Helpers;

public class UpdateHelper
{
    public static Message CallbackQueryToMessage(CallbackQuery callbackQuery)
    {
        //TODO если реализовать работу в группах - обработать
        return new Message
        {
            Text = callbackQuery.Data, 
            From = callbackQuery.From,
            Chat = new Chat 
            {
                Id = callbackQuery.From.Id
            }
        };
    }
}
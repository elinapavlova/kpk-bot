using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Common.Consts.Keyboards;

public static class VerifyStudentKeyboard
{
    public static IEnumerable<InlineKeyboardButton> Get(Guid requestId)
    {
        return new List<InlineKeyboardButton>
        {
            new ()
            {
                Text = "Подтвердить", CallbackData =  $"/auth_{requestId}_{RequestActions.Accept}"
            },
            new ()
            {
                Text = "Отказать", CallbackData = $"/auth_{requestId}_{RequestActions.Cancel}"
            }
        };
    }
}
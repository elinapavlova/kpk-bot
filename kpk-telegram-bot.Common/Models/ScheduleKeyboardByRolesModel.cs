using kpk_telegram_bot.Common.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Common.Models;

public class ScheduleKeyboardByRolesModel
{
    public ScheduleKeyboardByRolesModel(List<UserRole> roles, KeyValuePair<string, string> inlineButtonData)
    {
        Roles = roles;
        InlineButton = new InlineKeyboardButton
        {
            Text = inlineButtonData.Key, 
            CallbackData = inlineButtonData.Value
        };
    }

    public List<UserRole> Roles { get; set; }
    public InlineKeyboardButton InlineButton { get; set; }
}
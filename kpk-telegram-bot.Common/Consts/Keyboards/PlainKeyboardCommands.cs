using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Common.Consts.Keyboards;

public class PlainKeyboardCommands
{
    public static IEnumerable<InlineKeyboardButton> GetInlineButtons(UserRole userRole, Dictionary<Guid, string> subjects)
    {
        return All(subjects)
            .Where(x => x.Roles.Contains(userRole))
            .Select(x => x.InlineButton)
            .ToList();
    }

    private static IEnumerable<KeyboardByRolesModel> All(Dictionary<Guid, string> subjects)
    {
        return subjects
            .Select(x => new KeyboardByRolesModel
                (new List<UserRole> {UserRole.Admin}, new KeyValuePair<string, string>(x.Value, $"/plain_{x.Key}")))
            .ToList();
    }
}
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Common.Consts.Keyboards;

public static class ScheduleKeyboardCommands
{
    public static IEnumerable<InlineKeyboardButton> GetInlineButtons(UserRole userRole)
    {
        return All()
            .Where(x => x.Roles.Contains(userRole))
            .Select(x => x.InlineButton)
            .ToList();
    }

    private static IEnumerable<KeyboardByRolesModel> All()
    {
        var commands = new List<KeyboardByRolesModel>();
        commands.AddRange(new List<KeyboardByRolesModel>
        {
            new
            (
                new List<UserRole> {UserRole.Student, UserRole.Admin}, 
                new KeyValuePair<string, string>("Сегодня", "/schedule_today")
            ),
            new 
            (
                new List<UserRole> {UserRole.Student, UserRole.Admin}, 
                new KeyValuePair<string, string>("Актуальное", "/schedule_actual")
            ),           
            new 
            (
                new List<UserRole> {UserRole.Student, UserRole.Admin}, 
                new KeyValuePair<string, string>("Завтра", "/schedule_tomorrow")
            ),
            new 
            (
                new List<UserRole> {UserRole.Student, UserRole.Admin}, 
                new KeyValuePair<string, string>("Неделя", "/schedule_week")
            ),
            new 
            (
                new List<UserRole> {UserRole.Distant, UserRole.Admin},
                new KeyValuePair<string, string>("Заочники", "/schedule_distant")
            )
        });
        return commands;
    }
}
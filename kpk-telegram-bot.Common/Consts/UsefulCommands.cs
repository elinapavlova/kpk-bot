using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Models;

namespace kpk_telegram_bot.Common.Consts;

public static class UsefulCommands
{
    public static Dictionary<string, string> Commands(UserRole userRole)
    {
        return All()
            .Where(x => x.Roles.Contains(userRole))
            .Select(x => x.Command)
            .ToDictionary(x => x.Key, y => y.Value);
    }

    public static IEnumerable<CommandByRolesModel> All()
    {
        var commands = new List<CommandByRolesModel>();
        commands.AddRange(new List<CommandByRolesModel>
        {
            new 
            (
                new List<UserRole> {UserRole.Student}, 
                new KeyValuePair<string, string>("Мои оценки", "/marks")
            ),
            new 
            (
                new List<UserRole> {UserRole.Student}, 
                new KeyValuePair<string, string>("Мои долги", "/debts")
            ),           
            new 
            (
                new List<UserRole> {UserRole.Student}, 
                new KeyValuePair<string, string>("Следующая пара", "/next_pair")
            ),
            new 
            (
                new List<UserRole> {UserRole.Admin}, 
                new KeyValuePair<string, string>("Должники", "/debtors")
            ),
            new 
            (
                new List<UserRole> {UserRole.Admin},
                new KeyValuePair<string, string>("Оценки", "/marks")
            ),
            new 
            (
                new List<UserRole> {UserRole.Admin, UserRole.Student}, 
                new KeyValuePair<string, string>("Учебный план", "/plain")
            ),
            new 
            (
                new List<UserRole> {UserRole.Admin, UserRole.Student}, 
                new KeyValuePair<string, string>("Оценки группы", "/group_marks")
            ),
            new 
            (
                new List<UserRole> {UserRole.Admin, UserRole.Student}, 
                new KeyValuePair<string, string>("Расписание", "/schedule_inline_keyboard")
            )
        });
        return commands;
    }
}
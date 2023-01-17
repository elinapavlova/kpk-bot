namespace kpk_telegram_bot.Common.Consts;

public static class UsefulCommands
{
    public static readonly Dictionary<string, string> Commands = new ()
    {
        {"Мои оценки", "/marks" }, 
        {"Мои долги", "/dolgi"},
        {"Оценки группы", "/group_marks"},
        {"Расписание", "/schedule_inline_keyboard"},
        {"Следующая пара", "/next_pair"},
        {"Учебный план", "/plain"}
    };
}
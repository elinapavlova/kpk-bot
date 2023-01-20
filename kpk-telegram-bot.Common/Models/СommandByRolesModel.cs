using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Models;

public class CommandByRolesModel
{
    public CommandByRolesModel(List<UserRole> roles, KeyValuePair<string, string> command)
    {
        Roles = roles;
        Command = command;
    }

    public List<UserRole> Roles { get; set; }
    public KeyValuePair<string, string> Command { get; set; }
}
using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Models;

public record CommandByRolesModel(List<UserRole> Roles, KeyValuePair<string, string> Command)
{
    public List<UserRole> Roles { get; set; } = Roles;
    public KeyValuePair<string, string> Command { get; set; } = Command;
}
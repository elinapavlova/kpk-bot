using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Core.Commands;

namespace kpk_telegram_bot.Core.Helpers;

public static class RoleHelper
{
    public static bool CheckRole(uint? roleId, string command)
    {
        return roleId is not null && CommandAvailableRoles.GetValueOrDefault(command)?.Contains((UserRole)roleId) is true;
    }

    private static readonly Dictionary<string, List<UserRole>> CommandAvailableRoles = new()
    {
        {nameof(GroupCommand), new List<UserRole> {UserRole.Admin}},
        {nameof(ScheduleCommand), new List<UserRole> {UserRole.Admin, UserRole.Student, UserRole.Distant}}
    };
}
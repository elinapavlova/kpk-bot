using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Helpers;

public static class RoleHelper
{
    public static bool CheckRole(uint? roleId, string command)
    {
        return roleId is not null && CommandAvailableRoles.GetValueOrDefault(command)?.Contains((UserRole)roleId) is true;
    }

    private static readonly Dictionary<string, List<UserRole>> CommandAvailableRoles = new()
    {
        {nameof(GroupCommand), new List<UserRole> {UserRole.Admin}}
    };
}
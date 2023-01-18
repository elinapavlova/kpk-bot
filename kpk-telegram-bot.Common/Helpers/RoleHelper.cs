using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Helpers;

public static class RoleHelper
{
    public static bool CheckRole(UserEntity? user, List<UserRole> availableRoles)
    {
        return user is not null && availableRoles.Contains((UserRole)user.RoleId);
    }
}
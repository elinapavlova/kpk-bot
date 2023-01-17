using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Helpers;

public static class RoleHelper
{
    public static bool CheckRole(UserResponse? user, List<UserRole> availableRoles)
    {
        return user is not null && availableRoles.Contains(user.Role);
    }
}
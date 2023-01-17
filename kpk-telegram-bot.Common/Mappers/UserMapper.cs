using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Mappers;

public static class UserMapper
{
    public static UserResponse Map(UserEntity user)
    {
        return new UserResponse
        {
            Id = user.Id, UserName = user.UserName, GroupName = user.Group?.Name ?? "Не определена"
        };
    }
}
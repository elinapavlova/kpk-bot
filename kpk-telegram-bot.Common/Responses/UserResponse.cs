using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Responses;

public class UserResponse
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string GroupName { get; set; }
    public UserRole Role { get; set; }
}
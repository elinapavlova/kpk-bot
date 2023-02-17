namespace kpk_telegram_bot.Common.Models;

public record RequestCreateModel(string UserName, long UserId, string GroupName)
{
    public string UserName { get; set; } = UserName;
    public long UserId { get; set; } = UserId;
    public string GroupName { get; set; } = GroupName;
}
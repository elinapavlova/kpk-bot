namespace kpk_telegram_bot.Common.Responses;

public record RequestResponse
{
    public Guid RequestId { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public string GroupName { get; set; }
}
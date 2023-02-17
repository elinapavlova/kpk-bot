using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class RequestEntity : BaseEntity<Guid>
{
    public long UserId { get; set; }
    public string StudentName { get; set; }
    public Guid GroupId { get; set; }
    
    public ItemEntity Group { get; set; }
}
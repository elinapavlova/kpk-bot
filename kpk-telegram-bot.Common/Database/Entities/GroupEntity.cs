using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class GroupEntity : BaseEntity<Guid>
{
    public string Name { get; set; }
    
    public List<UserEntity> Users { get; set; }
}
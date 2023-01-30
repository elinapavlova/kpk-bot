using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class ItemEntity : BaseEntity<Guid>
{
    public Guid TypeId { get; set; }
    public ItemTypeEntity Type { get; set; }
    public List<ItemPropertyEntity> Properties { get; set; }
    public List<UserEntity>? Users { get; set; }
}
using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class ItemPropertyEntity : BaseEntity<Guid>
{
    public string Value { get; set; }
    
    public Guid TypeId { get; set; }
    public Guid ItemId { get; set; }
    
    public ItemPropertyTypeEntity Type { get; set; }
    public ItemEntity Item { get; set; }
}
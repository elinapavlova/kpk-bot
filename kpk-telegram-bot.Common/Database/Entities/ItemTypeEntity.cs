using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class ItemTypeEntity : BaseEntity<Guid>
{
    public string Name { get; set; }
    public string Value { get; set; }
    public List<ItemEntity> Items { get; set; }
    public List<ItemPropertyTypeEntity> PropertyTypes { get; set; }
}
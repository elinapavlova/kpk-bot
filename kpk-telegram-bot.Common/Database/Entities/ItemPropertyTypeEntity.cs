using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class ItemPropertyTypeEntity : BaseEntity<Guid>
{
    public string Name { get; set; }
    public string Value { get; set; }
    public List<ItemPropertyEntity> Properties { get; set; }
}
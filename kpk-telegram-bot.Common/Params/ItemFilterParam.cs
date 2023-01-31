using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Params;

public class ItemFilterParam
{
    public EntitiesState EntitiesState { get; set; }
    public Guid? TypeFilter { get; set; }
    public Dictionary<string, string>? PropertiesFilter { get; set; }
}
using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Params;

public class PropertyFilterParam
{
    public EntitiesState EntitiesState { get; set; }
    public Guid? TypeFilter { get; set; }
    public Guid? ItemFilter { get; set; }
    public string? ValueFilter { get; set; }
}
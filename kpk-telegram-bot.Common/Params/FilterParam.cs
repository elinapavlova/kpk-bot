using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Params;

public class FilterParam
{
    public EntitiesState EntitiesState { get; set; }
    public Dictionary<string, string>? ValuesFilter { get; set; }
}
using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Mappers;

public static class ScheduleTypeMapper
{
    public static ScheduleType Map(string type)
    {
        return type switch
        {
            "today" => ScheduleType.Today,
            "week" => ScheduleType.Week,
            "actual" => ScheduleType.Actual,
            "tomorrow" => ScheduleType.Tomorrow,
            "distant" => ScheduleType.Distant,
            _ => throw new Exception($"Невалидный тип расписания {type}")
        };
    }
}
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Extensions;
using kpk_telegram_bot.Common.Mappers;

namespace kpk_telegram_bot.Common.Helpers;

public static class ScheduleHelper
{
    public static void RemoveTempFiles(List<string> paths)
    {
        try
        {
            foreach (var path in paths)
            {
                File.Delete(path);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Не удалось удалить файл\r\n{ex}");
        }
    }

    public static IEnumerable<string> GetWeekDayNumbers()
    {
        var today = DateTime.Today;
        var weekDayNumbers = new List<string>();
        
        if (today.DayOfWeek == DayOfWeek.Monday)
        {
            for (var i = 0; i < 6; i++)
            {
                weekDayNumbers.Add(today.Day.ToString());
                today = today.NextDay();
            }
        }
        else
        {
            today = today.PreviousDayOfWeek(DayOfWeek.Monday);
            for (var i = 0; i < 6; i++)
            {
                weekDayNumbers.Add(today.Day.ToString());
                today = today.NextDay();
            } 
        }
        return weekDayNumbers;
    }

    public static string CreateMessageText(string type)
    {
        var scheduleType = ScheduleTypeMapper.Map(type);
        return scheduleType switch
        {
            ScheduleType.Actual => "Актуальное расписание",
            ScheduleType.Today => "Расписание на сегодня",
            ScheduleType.Week => "Расписание на неделю",
            ScheduleType.Tomorrow => "Расписание на завтра",
            _ => throw new Exception($"Невалидный тип расписания {type}")
        };
    }
}
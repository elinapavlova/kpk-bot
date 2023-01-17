namespace kpk_telegram_bot.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime PreviousDayOfWeek(this DateTime date, DayOfWeek day)
    {
        var diff = ((int)day - (int)date.DayOfWeek - 6) % 7;
        return date.AddDays(diff - 1);
    }

    public static DateTime NextDay(this DateTime date)
    {
        return date.AddDays(1);
    }
}
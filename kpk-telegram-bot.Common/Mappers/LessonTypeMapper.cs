using kpk_telegram_bot.Common.Enums;

namespace kpk_telegram_bot.Common.Mappers;

public static class LessonTypeMapper
{
    public static int Map(string lessonType)
    {
        return lessonType switch
        {
            "Л" => (int) LessonType.Lecture,
            "ПР" => (int) LessonType.Practice,
            "ПР СР" => (int) LessonType.IndependentPractice,
            "СР" => (int) LessonType.IndependentWork,
            "К" => (int) LessonType.Consultation,
            _ => (int) LessonType.Other
        };
    }
}
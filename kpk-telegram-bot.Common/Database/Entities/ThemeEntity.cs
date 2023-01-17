using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class ThemeEntity : BaseEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Homework { get; set; }
    public Guid SubjectId { get; set; }
    public uint LessonTypeId { get; set; }

    public SubjectEntity Subject { get; set; }
}
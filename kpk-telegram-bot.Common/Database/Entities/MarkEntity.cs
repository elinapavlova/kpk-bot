using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class MarkEntity : BaseEntity<Guid>
{
    public DateOnly Date { get; set; }
    public string Comment { get; set; }
    public string Mark { get; set; }
    public long UserId { get; set; }
    public Guid LessonId { get; set; }
    public uint MarkTypeId { get; set; }
    
    public UserEntity User { get; set; }
    public LessonEntity Lesson { get; set; }
}
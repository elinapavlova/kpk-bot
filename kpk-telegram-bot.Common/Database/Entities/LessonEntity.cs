using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class LessonEntity : BaseEntity<Guid>
{
    public DateOnly Date { get; set; }
    public string Pairs { get; set; }
    public uint? RoomNumber { get; set; }
    public string Comment { get; set; }
    public Guid GroupId { get; set; }
    public Guid ThemeId { get; set; }
    
    public GroupEntity Group { get; set; }
    public ThemeEntity Theme { get; set; }
    public List<MarkEntity> Marks { get; set; }
}
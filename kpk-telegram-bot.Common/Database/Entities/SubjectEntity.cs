using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class SubjectEntity : BaseEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public List<ThemeEntity> Themes { get; set; }
}
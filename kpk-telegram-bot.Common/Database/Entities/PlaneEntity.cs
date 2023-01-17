using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;
//TODO
public class PlaneEntity : BaseEntity<Guid>
{
    //public uint Wo
    public Guid SubjectId { get; set; }
    //public Guid 
    
    public SubjectEntity Subject { get; set; }
}
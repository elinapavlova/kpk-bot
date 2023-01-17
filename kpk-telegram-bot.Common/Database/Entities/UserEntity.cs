using kpk_telegram_bot.Common.Database.Base;

namespace kpk_telegram_bot.Common.Database.Entities;

public class UserEntity : BaseEntity<long>
{
    public string UserName { get; set; }
    public uint RoleId { get; set; }
    public Guid? GroupId { get; set; }
    
    public GroupEntity? Group { get; set; }
}
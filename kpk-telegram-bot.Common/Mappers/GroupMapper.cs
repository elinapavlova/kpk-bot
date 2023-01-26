using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Mappers;

public static class GroupMapper
{
    public static GroupResponse Map(GroupEntity group)
    {
        return new GroupResponse
        {
            Id = group.Id, Name = group.Name
        };
    }
    
    public static List<GroupResponse> Map(IEnumerable<GroupEntity> groups)
    {
        return groups.Select(Map).ToList();
    }
}
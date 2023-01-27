using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Mappers;

public static class ItemMapper
{
    public static ItemResponse Map(ItemEntity group)
    {
        return new ItemResponse
        {
            Id = group.Id, Name = group.Properties.FirstOrDefault(x => x.Type.Name.Equals("GroupName"))?.Value ?? "Ошибка"
        };
    }
    
    public static List<ItemResponse> Map(IEnumerable<ItemEntity> groups)
    {
        return groups.Select(Map).ToList();
    }
}
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Mappers;

public static class ItemPropertyMapper
{
    public static PropertyResponse Map(ItemPropertyEntity property)
    {
        return new PropertyResponse
        {
            Id = property.Id,
            Value = property.Value,
            Type = property.Type.Name
        };
    }

    public static List<PropertyResponse> Map(List<ItemPropertyEntity> properties)
    {
        return properties.Select(Map).ToList();
    }

    public static ItemPropertyEntity Map(Guid itemId, ItemPropertyCreateModel model)
    {
        return new ItemPropertyEntity
        {
            ItemId = itemId,
            IsDeleted = false,
            TypeId = model.TypeId,
            Value = model.Value
        };
    }
}
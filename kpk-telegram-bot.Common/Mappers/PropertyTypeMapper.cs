using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Mappers;

public static class PropertyTypeMapper
{
    public static PropertyTypeResponse Map(ItemPropertyTypeEntity type)
    {
        return new PropertyTypeResponse
        {
            Id = type.Id,
            Name = type.Name,
            Value = type.Value
        };
    }
    
    public static ItemPropertyTypeEntity Map(PropertyTypeResponse type)
    {
        return new ItemPropertyTypeEntity
        {
            Id = type.Id,
            Name = type.Name,
            Value = type.Value
        };
    }
}
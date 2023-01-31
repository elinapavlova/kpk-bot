using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Params;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Core.Services;

public class ItemPropertyService : IItemPropertyService
{
    private readonly IItemPropertyRepository _propertyRepository;
    private readonly IBaseService _baseService;
    
    public ItemPropertyService
    (
        IItemPropertyRepository propertyRepository, 
        IBaseService baseService
    )
    {
        _propertyRepository = propertyRepository;
        _baseService = baseService;
    }

    public async Task AddList(Guid itemId, List<ItemPropertyCreateModel> items)
    {
        var properties = await _baseService.Filter(new PropertyFilterParam
        {
            EntitiesState = EntitiesState.Actual,
            ItemFilter = itemId
        });
        
        foreach (var item in items)
        {
            var property = properties.FirstOrDefault(x => x.TypeId == item.TypeId);
            if (property is not null)
            {
                property.Value = item.Value;
                await _propertyRepository.Update(property);
                continue;
            }

            await _propertyRepository.Create(ItemPropertyMapper.Map(itemId, item));
        }
    }

    public async Task<PropertyResponse?> Update(ItemPropertyEntity item)
    {
        var result = await _propertyRepository.Update(item);
        return result is null
            ? null
            : ItemPropertyMapper.Map(result);
    }
}
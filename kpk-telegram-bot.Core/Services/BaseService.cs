using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Params;

namespace kpk_telegram_bot.Core.Services;

public class BaseService : IBaseService
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemTypeRepository _itemTypeRepository;
    private readonly IItemPropertyRepository _propertyRepository;

    public BaseService
    (
        IItemRepository itemRepository, 
        IItemTypeRepository itemTypeRepository, 
        IItemPropertyRepository propertyRepository
    )
    {
        _itemRepository = itemRepository;
        _itemTypeRepository = itemTypeRepository;
        _propertyRepository = propertyRepository;
    }
    
    public async Task<IQueryable<ItemEntity>> Filter(ItemFilterParam param)
    {
        var items = param.TypeFilter is null 
            ? await _itemRepository.GetAll() 
            : await _itemRepository.GetByTypeId((Guid)param.TypeFilter);
        
        items = param.EntitiesState switch
        {
            EntitiesState.Actual => items.Where(x => x.IsDeleted == false),
            EntitiesState.Deleted => items.Where(x => x.IsDeleted),
            _ => items
        };

        if (param.PropertiesFilter is not null)
        {
            foreach (var prop in param.PropertiesFilter)
            {
                items = items.Where(x => x.Properties
                    .Any(y => y.Type.Name == prop.Key && y.Value.Contains(prop.Value)));
            }
        }

        return items;
    }    
    
    public async Task<IQueryable<ItemPropertyEntity>> Filter(PropertyFilterParam param)
    {
        var items = param.TypeFilter is null 
            ? await _propertyRepository.GetAll() 
            : await _propertyRepository.GetByTypeId((Guid)param.TypeFilter);

        items = param.EntitiesState switch
        {
            EntitiesState.Actual => items.Where(x => x.IsDeleted == false),
            EntitiesState.Deleted => items.Where(x => x.IsDeleted),
            _ => items
        };

        if (param.ItemFilter is not null)
        {
            items = items.Where(x => x.ItemId == param.ItemFilter);
        }

        if (string.IsNullOrEmpty(param.ValueFilter) is false)
        {
            items = items.Where(x => x.Value.Contains(param.ValueFilter));
        }

        return items;
    }

    public async Task<Guid?> GetTypeIdByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        var item = await _itemTypeRepository.GetByName(name);
        return item?.Id;
    }
}
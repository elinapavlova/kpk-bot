using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Params;

namespace kpk_telegram_bot.Core.Services;

public class BaseService : IBaseService
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemTypeRepository _itemTypeRepository;

    public BaseService(IItemRepository itemRepository, IItemTypeRepository itemTypeRepository)
    {
        _itemRepository = itemRepository;
        _itemTypeRepository = itemTypeRepository;
    }
    
    public async Task<IQueryable<ItemEntity>> Filter(FilterParam param)
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
                    .Any(y => y.Value == prop.Value));
            }
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
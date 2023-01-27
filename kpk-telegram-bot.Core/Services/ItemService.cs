using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Params;
using kpk_telegram_bot.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Core.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IBaseService _baseService;

    public ItemService(IItemRepository itemRepository, IBaseService baseService)
    {
        _itemRepository = itemRepository;
        _baseService = baseService;
    }

    public async Task<ItemResponse?> GetByName(string typeName, string name)
    {
        name = name.Trim().ToUpper();
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        var filter = await _baseService.Filter(new FilterParam
        {
            EntitiesState = EntitiesState.Actual, 
            TypeFilter = await _baseService.GetTypeIdByName(name), 
            PropertiesFilter = new Dictionary<string, string>
            {
                {$"{typeName}Name", name}
            }
        });
        
        var result = await filter.FirstOrDefaultAsync();
        return result is null 
            ? null 
            : ItemMapper.Map(result);
    }

    public async Task<List<ItemResponse>?> GetAll(string name, bool onlyActual = true)
    {
        var result = await (await _baseService.Filter(new FilterParam
        {
            EntitiesState = onlyActual ? EntitiesState.Actual : EntitiesState.Deleted,
            TypeFilter = await _baseService.GetTypeIdByName(name)
        })).ToListAsync();
        
        return result.Count == 0
            ? null
            : ItemMapper.Map(result);
    }

    public async Task<ItemResponse?> Delete(string typeName, string name)
    {
        name = name.Trim().ToUpper();
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        var filter = await _baseService.Filter(new FilterParam
        {
            EntitiesState = EntitiesState.Actual,
            TypeFilter = await _baseService.GetTypeIdByName(name),
            PropertiesFilter = new Dictionary<string, string>
            {
                {$"{typeName}Name", name}
            }
        });

        var item = await filter.FirstOrDefaultAsync();
        if (item is null)
        {
            return null;
        }

        item.IsDeleted = true;
        item.DateDeleted ??= DateTime.Now;

        var result = await _itemRepository.Update(item);
        return result is null
            ? null
            : ItemMapper.Map(result);
    }
}
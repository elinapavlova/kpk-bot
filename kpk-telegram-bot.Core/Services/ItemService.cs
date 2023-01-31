using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Exceptions;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Params;
using kpk_telegram_bot.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Core.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IBaseService _baseService;
    private readonly IItemPropertyService _propertyService;
    private readonly IItemTypeRepository _typeRepository;
    private readonly ILogger _logger;

    public ItemService
    (
        IItemRepository itemRepository, 
        IBaseService baseService, 
        IItemPropertyService propertyService, ILogger logger, 
        IItemTypeRepository typeRepository
    )
    {
        _itemRepository = itemRepository;
        _baseService = baseService;
        _propertyService = propertyService;
        _logger = logger;
        _typeRepository = typeRepository;
    }

    public async Task<ItemResponse?> GetByName(string typeName, string name)
    {
        name = name.Trim();
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        var filter = await _baseService.Filter(new ItemFilterParam
        {
            EntitiesState = EntitiesState.Actual, 
            TypeFilter = await _baseService.GetTypeIdByName(typeName), 
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
        var result = await (await _baseService.Filter(new ItemFilterParam
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

        var filter = await _baseService.Filter(new ItemFilterParam
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

    public async Task<ItemResponse?> Create(ItemCreateModel model)
    {
        await CheckExisting(model.ParentId, model.TypeId);
        var existingItem = await CheckExistingLesson(model.TypeId, model.Properties.First(x => x.TypeId == Guid.Parse("0C850D51-4516-4191-9D5D-959F4026898A")).Value);

        await using var transaction = await _itemRepository.BeginTransaction();
        try
        {
            if (transaction is null)
            {
                throw new Exception("Не удалось запустить транзакцию");
            }

            if (existingItem is not null)
            {
                await UpdateProperties(model.Properties, existingItem.Properties);
                existingItem = await _itemRepository.Update(existingItem);
                return ItemMapper.Map(existingItem);
            }
        
            var item = await _itemRepository.Create(new ItemEntity
            {
                TypeId = model.TypeId, ParentId = model.ParentId
            });

            await _propertyService.AddList(item.Id, model.Properties);
            return ItemMapper.Map(await _itemRepository.GetById(item.Id));
        }
        catch (Exception exception)
        {
            _logger.Error("Не удалось добавить {item}\r\n{error}", model.ToString(), exception.Message);
            _logger.Debug("Item: {item}", model.ToString());
            if (transaction is not null)
            {
                transaction.HasError = true;
            }

            return null;
        }
    }

    #region Checking

    private async Task CheckExisting(Guid? parentId, Guid typeId)
    {
        if (parentId != null)
        {
            var parent = await _itemRepository.GetById((Guid) parentId);
            if (parent == null)
            {
                throw new NotFoundException($"Не удалось найти родительский Item по Id {parentId}",
                    nameof(NotFoundException));
            }
        }

        var modelType = await _typeRepository.GetById(typeId);
        if (modelType is null)
        {
            throw new NotFoundException($"Не удалось найти ItemType по Id {typeId}", nameof(NotFoundException));
        }
    }

    private async Task<ItemEntity?> CheckExistingLesson(Guid typeId, string lessonNumber)
    {
        var filter = await _baseService.Filter(new ItemFilterParam
        {
            EntitiesState = EntitiesState.Actual, TypeFilter = typeId, PropertiesFilter = new Dictionary<string, string>()
            {
                {"LessonNumber", lessonNumber}
            }
        });
        var result = await filter.FirstOrDefaultAsync();
        return result;
    }

    private async Task UpdateProperties(List<ItemPropertyCreateModel> propertiesForUpdate, List<ItemPropertyEntity> properties)
    {
        foreach (var item in properties)
        {
            var newValue = propertiesForUpdate.FirstOrDefault(x => x.TypeId == item.TypeId)?.Value;
            if (item.Value.Equals(newValue))
            {
                continue;
            }
            
            item.Value = newValue;
            await _propertyService.Update(item);
        }
    }
    
    #endregion
}
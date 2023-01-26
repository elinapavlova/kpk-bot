using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Params;
using kpk_telegram_bot.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace kpk_telegram_bot.Core.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<GroupResponse?> GetByName(string name)
    {
        name = name.Trim().ToUpper();
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        
        var result = await _groupRepository.GetByName(name);
        return result is null 
            ? null 
            : GroupMapper.Map(result);
    }

    public async Task<List<GroupResponse>?> GetAll()
    {
        var result = await (await _groupRepository.GetAll()).ToListAsync();
        
        return result is null || result.Count == 0
            ? null
            : GroupMapper.Map(result);
    }

    public async Task<GroupResponse?> Delete(string name)
    {
        var result = await _groupRepository.Delete(name);
        return result is null 
            ? null 
            : GroupMapper.Map(result);
    }
/*
    private void Filter(IQueryable<GroupEntity> items, FilterParam param)
    {
        items = param.EntitiesState switch
        {
            EntitiesState.Actual => items.Where(x => x.DateDeleted == null),
            EntitiesState.Deleted => items.Where(x => x.DateDeleted != null),
            _ => items
        };

        if (param.ValuesFilter is not null)
        {
            items.Tr
        }
    }
    */
}
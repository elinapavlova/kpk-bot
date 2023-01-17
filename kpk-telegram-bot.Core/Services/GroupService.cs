using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Responses;

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
}
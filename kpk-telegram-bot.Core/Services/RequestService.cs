using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Exceptions;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Responses;
using kpk_telegram_bot.Core.Helpers;

namespace kpk_telegram_bot.Core.Services;

public class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUserService _userService;
    private readonly IItemService _groupService;
    private readonly ILogger _logger;
    
    private const string GroupNameProperty = "groupName";

    public RequestService
    (
        IRequestRepository requestRepository, 
        IItemService groupService, 
        IUserService userService, 
        ILogger logger
    )
    {
        _requestRepository = requestRepository;
        _groupService = groupService;
        _userService = userService;
        _logger = logger;
    }

    public async Task<RequestEntity?> GetById(Guid requestId)
    {
        return await _requestRepository.GetById(requestId);
    }

    private async Task<RequestEntity?> Delete(Guid requestId)
    {
        return await _requestRepository.Delete(requestId);
    }

    public async Task<RequestResponse?> Accept(RequestEntity request)
    {
        await using var transaction = await _requestRepository.BeginTransaction();
        try 
        {
            var groupName = request.Group.Properties.FirstOrDefault(x => x.Type.Name == GroupNameProperty)?.Value;

            var user = await _userService.CreateOrUpdate(new UserEntity
            {
                Id = request.UserId, UserName = request.StudentName, GroupId = request.GroupId,
                RoleId = RoleHelper.GetUserRoleId(groupName)
            });

            if (user is null)
            {
                CommandHelper.ThrowException(
                    $"Не удалось добавить пользователя {request.UserId} [{request.StudentName} {groupName}]", "");
            }

            await Delete(request.Id);
            return RequestMapper.Map(request, GroupNameProperty);
        }
        catch (Exception exception)
        {
            _logger.Error("Не удалось добавить {item}\r\n{error}", request.ToString(), exception.Message);
            _logger.Debug("Item: {item}", request.ToString());
            if (transaction is not null)
            {
                transaction.HasError = true;
            }

            return null;
        }
    }
    
    public async Task<RequestResponse?> Cancel(Guid requestId)
    {
        var result = await Delete(requestId);
        return result is null 
            ? null 
            : RequestMapper.Map(result, GroupNameProperty);
    }

    public async Task<RequestEntity> Create(RequestCreateModel model)
    {
        var group = await CheckGroupExisting(model.GroupName);
        var request = RequestMapper.Map(model);
        
        request.GroupId = group.Id;
        
        var result = await _requestRepository.Create(request);
        return result;
    }

    private async Task<ItemResponse> CheckGroupExisting(string groupName)
    {
        var group = await _groupService.GetByName(ItemTypeNames.Group, groupName);
        if (group is null)
        {
            throw new NotFoundException("Несуществующая группа! Попробуйте еще раз");
        }

        return group;
    }
}
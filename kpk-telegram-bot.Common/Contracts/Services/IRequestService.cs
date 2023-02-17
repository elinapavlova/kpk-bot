using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IRequestService
{
    Task<RequestEntity?> GetById(Guid requestId);
    Task<RequestEntity> Create(RequestCreateModel model);
    Task<RequestResponse?> Accept(RequestEntity request);
    Task<RequestResponse?> Cancel(Guid requestId);
}
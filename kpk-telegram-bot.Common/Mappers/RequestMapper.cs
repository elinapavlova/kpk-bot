using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Common.Mappers;

public static class RequestMapper
{
    public static RequestEntity Map(RequestCreateModel model)
    {
        return new RequestEntity
        {
            UserId = model.UserId,
            StudentName = model.UserName
        };
    }

    public static RequestResponse? Map(RequestEntity request, string groupNameProperty)
    {
        return new RequestResponse
        {
            RequestId = request.Id,
            UserId = request.UserId,
            UserName = request.StudentName,
            GroupName = request.Group.Properties.FirstOrDefault(x => x.Type.Name == groupNameProperty)?.Value ?? "Не опеределена"
        };
    }
}
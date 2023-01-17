using Google.Apis.Drive.v3;
using File = Google.Apis.Drive.v3.Data.File;

namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IGoogleDriveService
{
    Task<IList<File>?> GetFiles(string? query);
    Task<FilesResource.GetRequest> GetFileById(string fileId);
}
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Options;
using File = Google.Apis.Drive.v3.Data.File;

namespace kpk_telegram_bot.Core.Services;

public class GoogleDriveService : IGoogleDriveService
{
    private readonly string _applicationName;
    private readonly string _apiKey;
    private readonly ILogger _logger;

    public GoogleDriveService
    (
        ApplicationOptions appOptions, 
        GoogleDriveApiOptions driveOptions, 
        ILogger logger
    )
    {
        _applicationName = appOptions.ApplicationName;
        _apiKey = driveOptions.ApiKey;
        _logger = logger;
    }

    public async Task<FilesResource.GetRequest> GetFileById(string fileId)
    {
        var service = Initialize();
        return service.Files.Get(fileId);
    }

    public async Task<IList<File>?> GetFiles(string? query)
    {
        try
        {
            var service = Initialize();
            var list = service.Files.List();
            list.Q = query;
            list.PageSize = 100;
            var result = await list.ExecuteAsync();
            return result?.Files;
        }
        catch (TaskCanceledException e)
        {
            _logger.Error($"Не удалось получить список файлов по запросу [{query}]\r\n{e}");
            return null;
        }
    }
    
    private DriveService Initialize()
    {
        var service = new DriveService(new BaseClientService.Initializer
        {
            ApplicationName = _applicationName,
            ApiKey = _apiKey
        });
        service.HttpClient.Timeout = TimeSpan.FromSeconds(30);
        
        return service;
    }
}
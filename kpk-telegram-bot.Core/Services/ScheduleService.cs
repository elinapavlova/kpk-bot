﻿using Google.Apis.Drive.v3;
using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Consts.GoogleDrive;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Options;
using SautinSoft.Document;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;

namespace kpk_telegram_bot.Core.Services;

public class ScheduleService : IScheduleService
{
    private readonly IGoogleDriveService _googleDriveService;
    private readonly string _archiveScheduleFolderId;
    private readonly string _actualScheduleFolderId;
    private readonly string _distantScheduleFolderId;
    private readonly string _basePath;

    public ScheduleService(IGoogleDriveService googleDriveService, ScheduleInfoOptions scheduleInfoOptions)
    {
        _googleDriveService = googleDriveService;
        _archiveScheduleFolderId = scheduleInfoOptions.ArchiveScheduleFolderId;
        _actualScheduleFolderId = scheduleInfoOptions.ActualScheduleFolderId;
        _distantScheduleFolderId = scheduleInfoOptions.DistanceScheduleFolderId;
        _basePath = scheduleInfoOptions.BasePath;
    }
    
    public async Task<List<InputOnlineFile>?> GetSchedule(ScheduleType type)
    {
        return type switch
        {
            ScheduleType.Actual => await GetActualSchedule(),
            ScheduleType.Today => await GetScheduleForDate(DateTime.Today),
            ScheduleType.Week => await GetWeekSchedule(),
            ScheduleType.Tomorrow => await GetScheduleForDate(DateTime.Today.AddDays(1)),
            ScheduleType.Distant => await GetDistantSchedule()
        };
    }

    private async Task<List<InputOnlineFile>?> GetScheduleForDate(DateTime date)
    {
        var fileName = Path.Combine(_basePath, $"{date.Day}.{date.Month}");

        if (File.Exists(GetFileName(fileName, FileTypes.Jpg)))
        {
            return new List<InputOnlineFile>
            {
                new (OpenFileForRead(fileName, FileTypes.Jpg), GetFileName(fileName, FileTypes.Jpg))
            };
        }
        
        var query = GoogleDriveQueryBuilder.Build(new Dictionary<GoogleDriveQueryParameterType, KeyValuePair<string, string>>
        {
            {GoogleDriveQueryParameterType.NotEquals, new KeyValuePair<string, string>("mimeType", GoogleDriveMimeTypes.Folder)},
            {GoogleDriveQueryParameterType.In, new KeyValuePair<string, string>("parents", _actualScheduleFolderId)},
            {GoogleDriveQueryParameterType.Contains, new KeyValuePair<string, string>("name", date.Day.ToString())},
        });
        
        var schedule = await _googleDriveService.GetFiles(query);
        if (schedule is null || schedule.Count is 0)
        {
            return null;
        }

        var file = schedule.First();
        var request = await _googleDriveService.GetFileById(file.Id);
        await CreateScheduleImage(fileName, request, file.MimeType);

        return new List<InputOnlineFile>
        {
            new (OpenFileForRead(fileName, FileTypes.Jpg), GetFileName(fileName, FileTypes.Jpg))
        };
    }

    private async Task<List<InputOnlineFile>?> GetWeekSchedule()
    {
        var actualSchedule = await GetActualSchedule();
        var archiveSchedule = await GetArchiveSchedule();
        var weekDayNumbers = ScheduleHelper.GetWeekDayNumbers();
        var dictionary = weekDayNumbers.ToDictionary<string?, string, InputOnlineFile?>(x => x, _ => null);

        foreach (var x in dictionary)
        {
            var file = GetFile(x.Key, actualSchedule, archiveSchedule);
            if (file is not null)
            {
                dictionary[x.Key] = file;
            }
        }
        
        return dictionary.Values
            .Where(x => x != null)
            .OrderBy(x => x.FileName)
            .ToList();
    }

    private static InputOnlineFile? GetFile(string number, IEnumerable<InputOnlineFile>? actualSchedule, 
        IEnumerable<InputOnlineFile> archiveSchedule)
    {
        var file = actualSchedule?.FirstOrDefault(y => y.FileName.StartsWith(number));
        if (file is not null)
        {
            return file;
        }  
            
        file = archiveSchedule.FirstOrDefault(y => y.FileName.StartsWith(number));
        return file ?? null;
    }

    private async Task<List<InputOnlineFile>?> GetDistantSchedule()
    {
        var query = GoogleDriveQueryBuilder.Build(new Dictionary<GoogleDriveQueryParameterType, KeyValuePair<string, string>>
        {
            {GoogleDriveQueryParameterType.NotEquals, new KeyValuePair<string, string>("mimeType", GoogleDriveMimeTypes.Folder)},
            {GoogleDriveQueryParameterType.In, new KeyValuePair<string, string>("parents", _distantScheduleFolderId)}
        });

        var schedule = await _googleDriveService.GetFiles(query);
        if (schedule is null || schedule.Count is 0)
        {
            return null;
        }
        
        var file = schedule.First();
        var fileName = Path.Combine(_basePath,file.Name.Split(".pdf").First());
        
        if (File.Exists(GetFileName(fileName, FileTypes.Jpg)) is false)
        {
            if (string.IsNullOrEmpty(file.Id))
            {
                return null;
            }
            var request = await _googleDriveService.GetFileById(file.Id);
            await CreateScheduleImage(fileName, request, file.MimeType);
        }
        
        var result = new InputOnlineFile(OpenFileForRead(fileName, FileTypes.Jpg), 
            GetFileName(file.Name, FileTypes.Jpg));
        return new List<InputOnlineFile> { result };
    }

    private async Task<List<InputOnlineFile>?> GetActualSchedule()
    {
        var query = GoogleDriveQueryBuilder.Build(new Dictionary<GoogleDriveQueryParameterType, KeyValuePair<string, string>>
        {
            {GoogleDriveQueryParameterType.NotEquals, new KeyValuePair<string, string>("mimeType", GoogleDriveMimeTypes.Folder)},
            {GoogleDriveQueryParameterType.In, new KeyValuePair<string, string>("parents", _actualScheduleFolderId)}
        });
        
        var schedule = await _googleDriveService.GetFiles(query);
        if (schedule is null || schedule.Count is 0)
        {
            return null;
        }
        
        var names = schedule.Select(x => x.Name.Split(' ').First());
        var fileNames = names.ToDictionary(x => x, x => Path.Combine(_basePath, $"{x}.{DateTime.Today.Month}"));
        var files = new List<InputOnlineFile>();

        foreach (var x in fileNames)
        {
            if (File.Exists(GetFileName(x.Value, FileTypes.Jpg)) is false)
            {
                var file = schedule.FirstOrDefault(f => f.Name.StartsWith(x.Key));
                if (string.IsNullOrEmpty(file?.Id))
                {
                    continue;
                }
                var request = await _googleDriveService.GetFileById(file.Id);
                await CreateScheduleImage(x.Value, request, file.MimeType);
            }
            files.Add(new InputOnlineFile(OpenFileForRead(x.Value, FileTypes.Jpg), GetFileName(x.Key, FileTypes.Jpg)));
        }

        return files.OrderBy(x => x.FileName).ToList();
    }
    
    private async Task<List<InputOnlineFile>> GetArchiveSchedule()
    {
        var folderId = await GetArchiveScheduleFolderId();
        var schedule = await GetArchiveScheduleDays(folderId);
        var names = schedule?.Select(x => x.Name.Split(' ').First());
        var fileNames = names.ToDictionary(x => x, x => Path.Combine(_basePath, $"{x}.{DateTime.Today.Month}"));
        var files = new List<InputOnlineFile>();

        foreach (var x in fileNames)
        {
            if (File.Exists(GetFileName(x.Value, FileTypes.Jpg)) is false)
            {
                var file = schedule.FirstOrDefault(f => f.Name.Contains(x.Key));
                var request = await _googleDriveService.GetFileById(file.Id);
                await CreateScheduleImage(x.Value, request, file.MimeType);
            }
            files.Add(new InputOnlineFile(OpenFileForRead(x.Value, FileTypes.Jpg), GetFileName(x.Key, FileTypes.Jpg)));
        }

        return files.OrderBy(x => x.FileName).ToList();
    }

    private async Task<string> GetArchiveScheduleFolderId()
    {
        var monthNumber = DateTime.Today.Month < 10 ? $"0{DateTime.Today.Month}" : DateTime.Today.Month.ToString();
        var query = GoogleDriveQueryBuilder.Build(new Dictionary<GoogleDriveQueryParameterType, KeyValuePair<string, string>>
        {
            {GoogleDriveQueryParameterType.Equals, new KeyValuePair<string, string>("mimeType", GoogleDriveMimeTypes.Folder)},
            {GoogleDriveQueryParameterType.In, new KeyValuePair<string, string>("parents", _archiveScheduleFolderId)},
            {GoogleDriveQueryParameterType.Contains, new KeyValuePair<string, string>("name", monthNumber)}
        });

        var schedule = await _googleDriveService.GetFiles(query);
        return schedule.First().Id;
    }    
    
    private async Task<IList<Google.Apis.Drive.v3.Data.File>?> GetArchiveScheduleDays(string folderId)
    {
        var query = GoogleDriveQueryBuilder.Build(new Dictionary<GoogleDriveQueryParameterType, KeyValuePair<string, string>>
        {
            {GoogleDriveQueryParameterType.NotEquals, new KeyValuePair<string, string>("mimeType", GoogleDriveMimeTypes.Folder)},
            {GoogleDriveQueryParameterType.In, new KeyValuePair<string, string>("parents", folderId)}
        });
        var schedule = await _googleDriveService.GetFiles(query);
        return schedule;
    }
    
    private static async Task CreateScheduleImage(string fileName, FilesResource.GetRequest file, string mimeType)
    {
        await using (var fileStream = new FileStream(GetFileName(fileName, FileTypeMapper.Map(mimeType)), FileMode.Create, FileAccess.Write))
        {
            file.Download(fileStream);
        }

        var dc = DocumentCore.Load(GetFileName(fileName, FileTypeMapper.Map(mimeType)));
        dc.Save(GetFileName(fileName, FileTypes.Jpg));

        ScheduleHelper.RemoveTempFiles(new List<string>
        {
            GetFileName(fileName, FileTypeMapper.Map(mimeType))
        });
    }

    private static FileStream OpenFileForRead(string path, string fileType)
    {
        return File.OpenRead($"{path}{fileType}");
    }

    private static string GetFileName(string fileName, string fileType)
    {
        return $"{fileName}{fileType}";
    }
}
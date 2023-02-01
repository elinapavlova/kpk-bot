using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Exceptions;
using kpk_telegram_bot.Common.Logger;
using kpk_telegram_bot.Common.Mappers;
using kpk_telegram_bot.Common.Models;
using kpk_telegram_bot.Common.Responses;
using OfficeOpenXml;

namespace kpk_telegram_bot.Core.Import;

public class PlaneReader : IReader
{
    private readonly IItemService _itemService;
    private readonly IItemTypeRepository _typeRepository;
    private readonly IItemPropertyTypeRepository _propertyTypeRepository;
    private readonly ILogger _logger;
    
    private const string LessonItemType = "Lesson";
    private const string SubjectItemType = "Subject";

    public PlaneReader
    (
        IItemService itemService, 
        IItemTypeRepository typeRepository, 
        IItemPropertyTypeRepository propertyTypeRepository, ILogger logger)
    {
        _itemService = itemService;
        _typeRepository = typeRepository;
        _propertyTypeRepository = propertyTypeRepository;
        _logger = logger;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task Import(MemoryStream stream)
    {
        using var package = new ExcelPackage(stream);
        var workSheet = package.Workbook.Worksheets.First();
        var totalRows = workSheet.Dimension.Rows;
        
        var subjectType = await _typeRepository.GetByName(SubjectItemType.ToLower()) 
                          ?? throw new NotFoundException("Не удалось найти ItemType {name}", SubjectItemType.ToLower());
        
        var lessonType = await _typeRepository.GetByName(LessonItemType.ToLower())
                         ?? throw new NotFoundException("Не удалось найти ItemType {name}", LessonItemType.ToLower());
        
        //TODO Передавать название дисциплины сообщением
        var subject = await GetSubject(subjectType.Id, package.Workbook.Worksheets.First().Name);

        var lessons = subject.Items?.Where(x => x.Type == LessonItemType);
        
        var lessonNamePropertyType = await _propertyTypeRepository.GetByName($"{LessonItemType}Name") 
                                     ?? throw new NotFoundException($"Не удалось найти свойство {LessonItemType}Name");
        var lessonNumberPropertyType = await _propertyTypeRepository.GetByName($"{LessonItemType}Number")
                                       ?? throw new NotFoundException($"Не удалось найти свойство {LessonItemType}Number");
        var lessonTypePropertyType = await _propertyTypeRepository.GetByName($"{LessonItemType}Type")
                                     ?? throw new NotFoundException($"Не удалось найти свойство {LessonItemType}Type");
        var lessonDescriptionPropertyType = await _propertyTypeRepository.GetByName($"{LessonItemType}Description")
                                            ?? throw new NotFoundException($"Не удалось найти свойство {LessonItemType}Description");;
        
        for (var i = 2; i <= totalRows; i++)
        {
            var number = workSheet.Cells[i, 1].Value?.ToString();
            if (number is not null && lessons?.Any(x => x.Type == $"{LessonItemType}Number" && x.Name == number) is true)
            {
                
            }
            var list = new List<ItemPropertyCreateModel>();
            list.AddRange(new []
            {
                new ItemPropertyCreateModel(lessonNamePropertyType.Id, workSheet.Cells[i, 2].Value?.ToString()),
                new ItemPropertyCreateModel(lessonNumberPropertyType.Id, number),
                new ItemPropertyCreateModel(lessonTypePropertyType.Id, LessonTypeMapper.Map(workSheet.Cells[i, 3].Value?.ToString()).ToString()),
            });
            if (workSheet.Cells[i, 4].Value?.ToString() != null)
            {
                list.Add(new ItemPropertyCreateModel(lessonDescriptionPropertyType.Id, workSheet.Cells[i, 4].Value?.ToString()));
            }
            
            var lessonCreateModel = new ItemCreateModel
            {
                ParentId = subject.Id, TypeId = lessonType.Id, Properties = list
            };

            await _itemService.Create(lessonCreateModel);
            _logger.Information("Добавлено занятие {number} {theme} для дисциплины {subject}", 
                number, workSheet.Cells[i, 2].Value?.ToString(), subject.Name);
        }
    }

    private async Task<ItemResponse> GetSubject(Guid subjectTypeId, string subjectName)
    {
        var subjectNamePropertyType = await _typeRepository.GetByName($"{SubjectItemType}Name");

        var subject = await _itemService.GetByName(SubjectItemType, subjectName)
                      ?? await _itemService.Create(new ItemCreateModel
                      {
                          TypeId = subjectTypeId, 
                          Properties = new List<ItemPropertyCreateModel>
                          {
                              new (subjectNamePropertyType.Id, subjectName)
                          }
                      })
                      ?? throw new CommandExecuteException("Не удалось добавить дисциплину {name}", subjectName);

        return subject;
    }
}
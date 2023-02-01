using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
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
    private readonly IItemPropertyService _propertyService;
    private readonly ILogger _logger;
    
    private const string LessonItemType = "Lesson";
    private const string SubjectItemType = "Subject";

    public PlaneReader
    (
        IItemService itemService, 
        IItemTypeRepository typeRepository, 
        IItemPropertyTypeRepository propertyTypeRepository, ILogger logger, IItemPropertyService propertyService)
    {
        _itemService = itemService;
        _typeRepository = typeRepository;
        _propertyTypeRepository = propertyTypeRepository;
        _logger = logger;
        _propertyService = propertyService;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task Import(string subjectName, MemoryStream stream)
    {
        using var package = new ExcelPackage(stream);
        
        var workSheet = package.Workbook.Worksheets.First();

        var subject = await GetSubject(subjectName);

        var lessonType = await _typeRepository.GetByName(LessonItemType.ToLower())
                         ?? throw new NotFoundException("Не удалось найти ItemType {name}", LessonItemType.ToLower());

        var lessonPropertyTypes = await _propertyTypeRepository.GetByItemTypeId(lessonType.Id);
        
        var lessons = subject.Items?.Where(x => x.Type == LessonItemType.ToLower()).ToList();
        
        var createPropertiesModel = CreateProperties(lessonPropertyTypes, workSheet);

        for (var i = 2; i <= workSheet.Dimension.Rows; i++)
        {
            var number = workSheet.Cells[i, 1].Value?.ToString();
            var properties = GetProperties(createPropertiesModel, i, number);

            var existingLesson = lessons?.FirstOrDefault(x => x.Properties
                .Any(y => y.Type.Name == $"{LessonItemType}Number" && y.Value == number));
            if (number is not null && existingLesson is not null)
            {
                await UpdateProperties(properties, ItemPropertyMapper.Map(existingLesson.Properties));
                _logger.Information("Обновлено занятие {number} {theme} для дисциплины {subject}", 
                    number, workSheet.Cells[i, 2].Value?.ToString(), subject.Name);
                return;
            }

            var lessonCreateModel = new ItemCreateModel(lessonType.Id, properties, subject.Id);

            await _itemService.Create(lessonCreateModel);
            _logger.Information("Добавлено занятие {number} {theme} для дисциплины {subject}", 
                number, workSheet.Cells[i, 2].Value?.ToString(), subject.Name);
        }
    }

    private static CreatePropertiesModel CreateProperties(List<ItemPropertyTypeEntity> lessonPropertyTypes, ExcelWorksheet workSheet)
    {
        var createPropertiesModel = new CreatePropertiesModel(
            lessonPropertyTypes.FirstOrDefault(x => x.Name.Equals($"{LessonItemType}Name"))?.Id,
            workSheet.Cells,
            lessonPropertyTypes.FirstOrDefault(x => x.Name.Equals($"{LessonItemType}Number"))?.Id,
            lessonPropertyTypes.FirstOrDefault(x => x.Name.Equals($"{LessonItemType}Type"))?.Id,
            lessonPropertyTypes.FirstOrDefault(x => x.Name.Equals($"{LessonItemType}Description"))?.Id);
        
        return createPropertiesModel;
    }

    private static List<ItemPropertyCreateModel> GetProperties(CreatePropertiesModel model, int i, string? number)
    {
        var properties = new List<ItemPropertyCreateModel>
        {
            new(model.NameTypeId, model.Cells[i, 2].Value?.ToString()),
            new(model.NumberTypeId, number),
            new(model.LessonTypeTypeId, LessonTypeMapper.Map(model.Cells[i, 3].Value?.ToString()).ToString())
        };

        if (model.Cells[i, 4].Value?.ToString() != null)
        {
            properties.Add(new ItemPropertyCreateModel(model.DescriptionTypeId, model.Cells[i, 4].Value?.ToString()));
        }

        return properties;
    }

    private async Task<ItemResponse> GetSubject(string subjectName)
    {
        var subjectType = await _typeRepository.GetByName(SubjectItemType.ToLower()) 
                          ?? throw new NotFoundException("Не удалось найти ItemType {name}", SubjectItemType.ToLower());
        
        var subject = await _itemService.GetByName(SubjectItemType, subjectName);
        if (subject is not null)
        {
            return subject;
        }
        
        var namePropertyType = await _typeRepository.GetByName($"{SubjectItemType}Name");

        subject = await _itemService.Create(new ItemCreateModel(subjectType.Id, new List<ItemPropertyCreateModel>
                                            {
                                                new (namePropertyType.Id, subjectName)
                                            })) 
                  ?? throw new CommandExecuteException("Не удалось добавить дисциплину {name}", subjectName);

        return subject;
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

    private class CreatePropertiesModel
    {
        public readonly Guid NameTypeId;
        public readonly ExcelRange Cells;
        public readonly Guid NumberTypeId;
        public readonly Guid LessonTypeTypeId;
        public readonly Guid DescriptionTypeId;

        public CreatePropertiesModel(Guid? nameTypeId, ExcelRange cells, Guid? numberTypeId, Guid? lessonTypeTypeId, Guid? descriptionTypeId)
        {
            NameTypeId = nameTypeId ?? throw new NotFoundException($"Не удалось найти свойство {LessonItemType}Name");
            Cells = cells;
            NumberTypeId = numberTypeId ?? throw new NotFoundException($"Не удалось найти свойство {LessonItemType}Number");
            LessonTypeTypeId = lessonTypeTypeId ?? throw new NotFoundException($"Не удалось найти свойство {LessonItemType}Type");
            DescriptionTypeId = descriptionTypeId ?? throw new NotFoundException($"Не удалось найти свойство {LessonItemType}Description");
        }
    }
}
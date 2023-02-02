using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database.Entities;
using kpk_telegram_bot.Common.Enums;
using kpk_telegram_bot.Common.Responses;

namespace kpk_telegram_bot.Core.Services;

public class PlainService : IPlainService
{
    private readonly IItemService _itemService;
    //TODO
    private const string SubjectItemType = "Subject";

    public PlainService(IItemService itemService)
    {
        _itemService = itemService;
    }

    public async Task<Dictionary<Guid, string>> GetSubjectNames()
    {
        var subjects = await GetSubjects();
        return subjects.ToDictionary(x => x.ItemId, entity => entity.Value);
    }

    public async Task<string> GetPlainBySubjectId(Guid subjectId)
    {
        var lessons = await GetLessons(subjectId);
        return CreateMessage(lessons);
    }
    
    private async Task<List<ItemPropertyEntity>?> GetSubjects()
    {
        var subjects = await _itemService.GetListByTypeName(SubjectItemType);
        return subjects;
    }

    private async Task<IEnumerable<ItemResponse>?> GetLessons(Guid subjectId)
    {
        var lessons = await _itemService.GetAll(SubjectItemType);
        return lessons?.Where(x => x.ParentId == subjectId).ToList();
    }

    private static string CreateMessage(IEnumerable<ItemResponse> lessons)
    {
        var result = string.Empty;
        
        var lectureHours = lessons.Count(x => x.Properties
            .FirstOrDefault(y => y.Type.Name == "LessonType")
            ?.Value == ((int)LessonType.Lecture).ToString()) * 2;
        if (lectureHours != 0)
        {
            result += $"Лекции (ч): {lectureHours}\r\n";
        }
        
        var consultationHours = lessons.Count(x => x.Properties
            .FirstOrDefault(y => y.Type.Name == "LessonType")
            ?.Value == ((int)LessonType.Consultation).ToString()) * 2;   
        if (consultationHours != 0)
        {
            result += $"Лекции (ч): {consultationHours}\r\n";
        }
        
        var practiceHours = lessons.Count(x => x.Properties
            .FirstOrDefault(y => y.Type.Name == "LessonType")
            ?.Value == ((int)LessonType.Practice).ToString()) * 2;     
        if (practiceHours != 0)
        {
            result += $"Практика (ч): {practiceHours}\r\n";
        }
        
        var independentWorkHours = lessons.Count(x => x.Properties
            .FirstOrDefault(y => y.Type.Name == "LessonType")
            ?.Value == ((int)LessonType.IndependentWork).ToString()) * 2;
        if (independentWorkHours != 0)
        {
            result += $"СР (ч): {independentWorkHours}\r\n";
        }
        
        var independentPracticeHours = lessons.Count(x => x.Properties
            .FirstOrDefault(y => y.Type.Name == "LessonType")
            ?.Value == ((int)LessonType.IndependentPractice).ToString()) * 2;
        if (independentPracticeHours != 0)
        {
            result += $"Практика (СР) (ч): {independentPracticeHours}\r\n";
        }
        
        var examHours = lessons.Count(x => x.Properties
            .FirstOrDefault(y => y.Type.Name == "LessonType")
            ?.Value == ((int)LessonType.Exam).ToString()) * 6;
        if (examHours != 0)
        {
            result += $"Экзамен (ч): {examHours}\r\n";
        }

        result += $"Всего (ч): {lectureHours + consultationHours + practiceHours + independentPracticeHours + independentWorkHours + examHours}";

        return result;
    }
}
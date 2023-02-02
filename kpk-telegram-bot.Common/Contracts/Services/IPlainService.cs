namespace kpk_telegram_bot.Common.Contracts.Services;

public interface IPlainService
{
    Task<Dictionary<Guid, string>> GetSubjectNames();
    Task<string> GetPlainBySubjectId(Guid subjectId);
}
namespace kpk_telegram_bot.Common.Contracts;

public interface IReader
{
    Task Import(string subjectName, MemoryStream stream);
}
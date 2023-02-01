namespace kpk_telegram_bot.Common.Contracts;

public interface IReader
{
    Task Import(MemoryStream stream);
}
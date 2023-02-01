using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Logger;
using Telegram.Bot.Types;

namespace kpk_telegram_bot.Core.Commands;

public class ImportCommand : ICommand
{
    private readonly IReader _reader;
    private readonly ITelegramHttpClient _telegramHttpClient;
    private readonly ILogger _logger;

    public ImportCommand(IReader reader, ITelegramHttpClient telegramHttpClient, ILogger logger)
    {
        _reader = reader;
        _telegramHttpClient = telegramHttpClient;
        _logger = logger;
    }
    
    public async Task Execute(Message message)
    {
        using var stream = new MemoryStream();
        await _telegramHttpClient.GetFileById(message.Document.FileId, stream);
        await _reader.Import(message.Caption, stream);
        await _telegramHttpClient.SendTextMessage(message.From.Id, "Импорт выполнен успешно");
        _logger.Information("Выполнен импорт файла {fileId} пользователем {userId} [{username}]", 
            message.Document.FileId, message.From.Id, message.From.Username);
    }
}
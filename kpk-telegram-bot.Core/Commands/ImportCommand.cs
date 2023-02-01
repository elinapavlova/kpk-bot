using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using Telegram.Bot.Types;

namespace kpk_telegram_bot.Core.Commands;

public class ImportCommand : ICommand
{
    private readonly IReader _reader;
    private readonly ITelegramHttpClient _telegramHttpClient;

    public ImportCommand(IReader reader, ITelegramHttpClient telegramHttpClient)
    {
        _reader = reader;
        _telegramHttpClient = telegramHttpClient;
    }
    
    public async Task Execute(Message message)
    {
        using var stream = new MemoryStream();
        var document = await _telegramHttpClient.GetFileById(message.Document.FileId, stream);
        await _reader.Import(stream);
    }
}
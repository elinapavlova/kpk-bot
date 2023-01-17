using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Core.HttpClients;

public class TelegramHttpClient : ITelegramHttpClient
{
    private readonly string _telegramApiToken;
    private TelegramBotClient _bot;

    public TelegramHttpClient(TelegramBotOptions telegramBotOptions)
    {
        _telegramApiToken = telegramBotOptions.Token;
    }

    public TelegramBotClient CreateBotClient()
    {
        _bot = new TelegramBotClient(_telegramApiToken);
        return _bot;
    }

    public async Task SendTextMessage(long chatId, string text, ReplyKeyboardMarkup keyboard, ParseMode parseMode = ParseMode.Default)
    {
        if (parseMode == ParseMode.Default)
        {
            await _bot.SendTextMessageAsync(chatId, text, replyMarkup: keyboard);
            return;
        }
            
        await _bot.SendTextMessageAsync(chatId, text, parseMode, replyMarkup: keyboard);
    }

    public async Task SendTextMessage(long chatId, string text, InlineKeyboardMarkup keyboard, ParseMode parseMode = ParseMode.Default)
    {
        if (parseMode == ParseMode.Default)
        {
            await _bot.SendTextMessageAsync(chatId, text, replyMarkup: keyboard);
            return;
        }
            
        await _bot.SendTextMessageAsync(chatId, text, parseMode, replyMarkup: keyboard);
    }

    public async Task SendTextMessage(long chatId, string text, int replyToMessageId = 0,
        ParseMode parseMode = ParseMode.Default, InlineKeyboardMarkup? keyboard = null)
    {
        await _bot.SendTextMessageAsync(chatId, text, parseMode, replyToMessageId: replyToMessageId,
            replyMarkup: keyboard);
    }

    public async Task EditTextMessage(long chatId, int messageId, string text)
    {
        await _bot.EditMessageTextAsync(chatId, messageId, text);
    }

    public async Task SendPhotoMessage(long groupChatId, string text, string url)
    {
        await _bot.SendPhotoAsync(groupChatId, url, text);
    }
    
    public async Task SendPhotoMessage(long chatId, string text, List<InputOnlineFile>? files)
    {
        var media = new List<IAlbumInputMedia>();
        var firstPhoto = files.First();
        
        media.Add(new InputMediaPhoto(new InputMedia(firstPhoto.Content, firstPhoto.FileName))
        {
            Caption = text
        });
        media.AddRange(files.Where(x => x.FileName != firstPhoto.FileName)
            .Select(x => new InputMediaPhoto(new InputMedia(x.Content, x.FileName))));

        await _bot.SendMediaGroupAsync(chatId, media);
    }
}
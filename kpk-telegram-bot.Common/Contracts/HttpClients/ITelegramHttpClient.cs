using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Common.Contracts.HttpClients;

public interface ITelegramHttpClient
{
    TelegramBotClient CreateBotClient();

    Task SendTextMessage(long chatId, string text, ReplyKeyboardMarkup keyboard,
        ParseMode parseMode = ParseMode.Default);    
    Task SendTextMessage(long chatId, string text, InlineKeyboardMarkup keyboard,
        ParseMode parseMode = ParseMode.Default);
    Task SendTextMessage(long chatId, string text, int replyToMessageId = 0,
        ParseMode parseMode = ParseMode.Default, InlineKeyboardMarkup? keyboard = null);
    Task EditTextMessage(long chatId, int messageId, string text);
    Task SendPhotoMessage(long groupChatId, string text, string url);
    Task SendPhotoMessage(long chatId, string text, List<InputOnlineFile>? files);
}
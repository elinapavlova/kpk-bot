using kpk_telegram_bot.Common.Consts;
using kpk_telegram_bot.Common.Consts.GoogleDrive;
using kpk_telegram_bot.Common.Exceptions;

namespace kpk_telegram_bot.Common.Mappers;

public static class FileTypeMapper
{
    public static string Map(string mimeType)
    {
        return mimeType switch
        {
            GoogleDriveMimeTypes.Docx => FileTypes.Docx,
            GoogleDriveMimeTypes.Pdf => FileTypes.Pdf,
            _ => throw new CommandExecuteException($"Тип файла из Google Drive {mimeType} не поддерживается", mimeType, null)
        };
    }
}
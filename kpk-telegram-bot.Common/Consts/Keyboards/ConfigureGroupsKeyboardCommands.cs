using kpk_telegram_bot.Common.Responses;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Common.Consts.Keyboards;

public static class ConfigureGroupsKeyboardCommands
{
    public static IEnumerable<InlineKeyboardButton> All(IEnumerable<ItemResponse> groups)
    {
        var buttons = List(groups);
        buttons.AddRange( new []
        {
            new InlineKeyboardButton
            {
                Text = "Добавить", CallbackData = "/group_create"
            },            
            new InlineKeyboardButton
            {
                Text = "Удаленные", CallbackData = "/group_deleted"
            },
        });

        return buttons;
    }
    
    public static List<InlineKeyboardButton> List(IEnumerable<ItemResponse> groups)
    {
        return groups
            .Select(x => new InlineKeyboardButton
            {
                Text = x.Name, CallbackData = $"/group_{x.Name}"
            })
            .ToList();
    }

    public static IEnumerable<InlineKeyboardButton> ForGroup(string groupName)
    {
        var buttons = new List<InlineKeyboardButton>
        {
            new ()
            {
                Text = "Список", CallbackData = $"/student_{groupName}"
            },
            new ()
            {
                Text = "Удалить", CallbackData = $"/group_{groupName}_delete"
            },
            new ()
            {
                Text = "Редактировать", CallbackData = $"/group_{groupName}_update"
            }
        };
        return buttons;
    }
}
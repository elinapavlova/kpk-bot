using kpk_telegram_bot.Common.Consts.Keyboards;
using kpk_telegram_bot.Common.Contracts.Commands;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Common.Logger;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace kpk_telegram_bot.Core.Commands;

public class GroupCommand : ICommand
{
    private readonly ITelegramHttpClient _telegramHttpClient;
    private readonly ILogger _logger;
    private readonly IGroupService _groupService;
    private const string GroupConfiguring = "Управление группами";

    public GroupCommand
    (
        ITelegramHttpClient telegramHttpClient, 
        ILogger logger,
        IGroupService groupService
    )
    {
        _telegramHttpClient = telegramHttpClient;
        _logger = logger;
        _groupService = groupService;
    }
    //TODO Работа с удаленными
    public async Task Execute(Message message)
    {
        var text = message.Text.Trim();
        var groups = await _groupService.GetAll();
        if (groups is null || groups.Count == 0)
        {
            CommandHelper.ThrowException("Не удалось найти список групп", text);
            _logger.Warning("Не удалось найти список групп. Пользователь {fromId} [{fromUsername}]", message.From.Id, message.From.Username);
            return;
        }
        
        if (text == GroupConfiguring)
        {
            await SendAnswer(message.From.Id, text, new InlineKeyboardMarkup(ConfigureGroupsKeyboardCommands.All(groups)));
            return;
        }
        
        var words = text.Split('_');
        switch (words.Length)
        {
            case < 2:
                CommandHelper.ThrowException("Невалидный запрос", text);
                return;
            case 2:
                await ConfigureGroupCommand(words[1], message);
                break;
            case 3:
                await ConfigureGroupCommand(words[1], words[2], message);
                break;
        }
    }

    #region Управление группами
    private async Task ConfigureGroupCommand(string command, Message message)
    {
        switch (command)
        {
            case "list":
            {
                await GetGroupList(message.From, command);
                break;
            }
            case "create":
            {
                await SendAnswer(message.From.Id, "Для добавления группы отправьте команду в виде: /group_create 1-1П9");
                break;
            }
            default:
            {
                await GetCommandByName(command, message.From);
                return;
            }
        }
    }
    
    private async Task ConfigureGroupCommand(string groupName, string command, Message message)
    {
        switch (command)
        {
            case "list":
            {
                //TODO переводить на /student_list
                break;
            }
            case "create":
            {
                await CreateGroup(groupName, command, message.From);
                break;
            }
            case "delete":
            {
                await DeleteGroup(groupName, command, message.From);
                break;
            }
            default:
            {
                await SendAnswer(message.From.Id, groupName, new InlineKeyboardMarkup(ConfigureGroupsKeyboardCommands.ForGroup(groupName)));
                break;
            }
        }
    }
    #endregion
    
    #region private
    
    private async Task SendAnswer(long chatId, string text, InlineKeyboardMarkup? keyboard = null)
    {
        if (keyboard is null)
        {
            await _telegramHttpClient.SendTextMessage(chatId, text);
            return;
        }
        
        await _telegramHttpClient.SendTextMessage(chatId, text, keyboard);
    }
    
    private async Task GetGroupList(User from, string command)
    {
        var groups = await _groupService.GetAll();
        if (groups is null || groups.Count == 0)
        {
            CommandHelper.ThrowException("Не удалось найти список групп", command);
            return;
        }

        await SendAnswer(from.Id, "Список групп", new InlineKeyboardMarkup(ConfigureGroupsKeyboardCommands.List(groups)));
    }

    private async Task DeleteGroup(string groupName, string command, User from)
    {
        var group = await _groupService.Delete(groupName);
        if (group is null)
        {
            CommandHelper.ThrowException($"Группа {groupName} не найдена", command);
            return;
        }

        await SendAnswer(from.Id, $"Группа {group.Name} успешно удалена");
        _logger.Warning("Группа {group} успешно удалена. Пользователь {fromId} [{fromUsername}]", group.Name, from.Id, from.Username);
    }
    
    private async Task GetCommandByName(string command, User from)
    {
        var group = await _groupService.GetByName(command);
        if (group is null)
        {
            CommandHelper.ThrowException($"Группа {command} не найдена", command);
            return;
        }

        await SendAnswer(from.Id, group.Name, new InlineKeyboardMarkup(ConfigureGroupsKeyboardCommands.ForGroup(group.Name)));
    }
    
    private async Task CreateGroup(string groupName, string command, User from)
    {
        var group = await _groupService.GetByName(groupName);
        if (group is not null)
        {
            CommandHelper.ThrowException($"Группа {groupName} уже существует", command);
            return;
        }

        await SendAnswer(from.Id, $"Группа {groupName} успешно добавлена");
        _logger.Information("Добавлена группа {groupName}. Пользователь {fromId} [{fromUsername}]", groupName, from.Id, from.Username);
    }
    #endregion
}
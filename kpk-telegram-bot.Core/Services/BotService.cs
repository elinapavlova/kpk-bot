using kpk_telegram_bot.Common.Contracts;
using kpk_telegram_bot.Common.Contracts.HttpClients;
using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Exceptions;
using kpk_telegram_bot.Common.Logger;
using Telegram.Bot;

namespace kpk_telegram_bot.Core.Services;

public class BotService : IBotService
{
    private readonly ILogger _logger;
    private readonly ITelegramHttpClient _telegramHttpClient;
    private readonly ITelegramBotHandler _telegramBotHandler;

    public BotService(ILogger logger, ITelegramHttpClient telegramHttpClient, ITelegramBotHandler telegramBotHandler)
    {
        _logger = logger;
        _telegramHttpClient = telegramHttpClient;
        _telegramBotHandler = telegramBotHandler;
    }
    
    public async Task Work()
    {
        try
        {
            using var cts = new CancellationTokenSource();
            InitializeBot(cts);
            _logger.Information($"kpk-telegram-bot started at {DateTime.Now} successfully");

            Console.ReadLine();
            cts.Cancel();
        }
        catch (Exception e)
        {
            _logger.Error($"App error: {e.Message}");
        }
    }

    #region initializing

    private void InitializeBot(CancellationTokenSource cts)
    {
        CheckServicesExist();
        var bot = _telegramHttpClient.CreateBotClient();
        bot.StartReceiving(_telegramBotHandler.CreateDefaultUpdateHandler(), cts.Token);
    }  
    
    private void CheckServicesExist()
    {
        if (_telegramHttpClient is null)
        {
            throw new AppException($"App error: {nameof(ITelegramHttpClient)} not found.");
        }        
        if (_telegramBotHandler is null)
        {
            throw new AppException($"App error: {nameof(ITelegramBotHandler)} not found.");
        }
    }

    #endregion
}